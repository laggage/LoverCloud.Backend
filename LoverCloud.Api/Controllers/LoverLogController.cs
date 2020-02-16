namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Api.Extensions;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Extensions;
    using LoverCloud.Infrastructure.Resources;
    using LoverCloud.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Authorize]
    [Route("api/lovers/logs")]
    public class LoverLogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoverLogRepository _repository;
        private readonly ILoverCloudUserRepository _userRepository;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public LoverLogController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoverLogRepository repository,
            ILoverCloudUserRepository userRepository,
            IPropertyMappingContainer propertyMappingContainer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
            _userRepository=userRepository;
            _propertyMappingContainer=propertyMappingContainer;
        }

        /// <summary>
        /// 获取情侣日志
        /// </summary>
        /// <returns>情侣日志</returns>
        [HttpGet(Name = "GetLoverLogs")]
        public async Task<IActionResult> Get([FromQuery]LoverLogParameters parameters)
        {
            string userId = this.GetUserId();
            PaginatedList<LoverLog> logs = 
                await _repository.GetLoverLogsAsync(userId, parameters);

            IQueryable<LoverLog> sortedLogs = logs.AsQueryable()
                .ApplySort(
                    parameters.OrderBy,
                    _propertyMappingContainer.Resolve<LoverLogResource, LoverLog>());

            IEnumerable<LoverLogResource> loverLogResources =
                _mapper.Map<IEnumerable<LoverLogResource>>(sortedLogs)
                .Select(x =>
                {
                    Parallel.ForEach(x.LoverPhotos, photo =>
                    {
                        photo.Url = Url.Link("GetPhoto", new { id = photo.Id });
                    });
                    return x;
                });
            

            IEnumerable<ExpandoObject> shapedLoverLogResources =
                loverLogResources.ToDynamicObject(parameters.Fields)
                .AddLinks(this, parameters.Fields, "GetLoverLog", "DeleteLoverLog", "PartiallyUpdateLoverLog");

            var result = new
            {
                value = shapedLoverLogResources,
                links = this.CreatePaginationLinks("GetLoverLogs", parameters, logs.HasPrevious, logs.HasNext)
            };

            this.AddPaginationHeaderToResponse(logs);

            return Ok(result);
        }

        /// <summary>
        /// 获取情侣日志
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetLoverLog")]
        public async Task<IActionResult> Get([FromRoute]string id, [FromQuery]string fields)
        {
            LoverLog loverLog = await _repository.FindByIdAsync(id);
            if (loverLog == null) return NotFound();

            string userId = this.GetUserId();
            if (!(loverLog?.Lover?.LoverCloudUsers.Any(user => user.Id == userId) ?? false))
                return Forbid();

            LoverLogResource loverLogResource = _mapper.Map<LoverLogResource>(loverLog);
            ExpandoObject shapedLoverLogResource = loverLogResource.ToDynamicObject(fields)
                .AddLinks(
                this, fields, "log", "GetLoverLogs", 
                "DeleteLoverLog", "PartiallyUpdateLoverLog");

            return Ok(shapedLoverLogResource);
        }

        /// <summary>
        /// 新建情侣日志
        /// </summary>
        /// <param name="addResource"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddLoverLog")]
        public async Task<IActionResult> Add([FromForm]LoverLogAddResource addResource)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            LoverCloudUser user = await _userRepository.FindByIdAsync(this.GetUserId());
            if (user.Lover == null) return this.UserNoLoverResult(user);
            Lover lover = user.Lover;

            LoverLog loverLog = _mapper.Map<LoverLog>(addResource);
            loverLog.Creater = user;
            loverLog.Lover = lover;
            loverLog.CreateDateTime = DateTime.Now;
            loverLog.LastUpdateTime = DateTime.Now;
            _repository.Add(loverLog);
            if(addResource.Photos != null)
                foreach(var formFile in addResource.Photos)
                {
                    var photo = new LoverPhoto
                    {
                        Name = formFile.FileName,
                        Uploader = user,
                        Lover = lover,
                        LoverLog = loverLog,
                        PhotoTakenDate = DateTime.Now,
                    };
                    photo.PhysicalPath = photo.GeneratePhotoPhysicalPath(formFile.GetFileSuffix());
                    loverLog.LoverPhotos.Add(photo);
                    await formFile.SaveToFileAsync(photo.PhysicalPath);
                }

            if (!await _unitOfWork.SaveChangesAsync()) return NoContent();

            LoverLogResource loverLogResource = _mapper.Map<LoverLogResource>(loverLog);
            ExpandoObject shapedLoverLogResource = loverLogResource.ToDynamicObject()
                .AddLinks(
                this, null, "log", "GetLoverLog", 
                "DeleteLoverLog", "PartiallyUpdateLoverLog");

            return CreatedAtRoute("AddLoverLog", shapedLoverLogResource);
        }

        /// <summary>
        /// 删除情侣日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteLoverLog")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            LoverLog loverLog = await _repository.FindByIdAsync(id);
            if (loverLog== null) return NotFound();

            LoverCloudUser user = await _userRepository.FindByIdAsync(this.GetUserId());
            if (user.Lover == null) return this.UserNoLoverResult(user);
            if (loverLog.LoverId != user.Lover.Id)
                return Forbid($"Id为:\"{this.GetUserId()}\"没有权限删除本资源");

            _repository.Delete(loverLog);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to delete resource");

            return NoContent();
        }

        /// <summary>
        /// 局部更新情侣日志
        /// </summary>
        /// <param name="id">情侣日志id</param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PartiallyUpdateLoverLog")]
        public async Task<IActionResult> PartiallyUpdate(
            [FromRoute]string id, [FromBody]JsonPatchDocument<LoverLogUpdateResource> patchDoc)
        {
            LoverLog loverLog = await _repository.FindByIdAsync(id);
            if (loverLog == null) return NotFound();
            if (loverLog.CreaterId != this.GetUserId())
                return Forbid();

            LoverLogUpdateResource loverLogUpdateResource = _mapper.Map<LoverLogUpdateResource>(loverLog);

            patchDoc.ApplyTo(loverLogUpdateResource);
            _mapper.Map(loverLogUpdateResource, loverLog);
            loverLog.LastUpdateTime = DateTime.Now;

            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to update lover log resource");
            return NoContent();
        }
    }
}
