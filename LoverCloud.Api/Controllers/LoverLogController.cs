namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Api.Extensions;
    using LoverCloud.Core.Extensions;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Extensions;
    using LoverCloud.Infrastructure.Resources;
    using LoverCloud.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
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
        private readonly ILoverRepository _loverRepository;
        private readonly ILoverCloudUserRepository _userRepository;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public LoverLogController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoverLogRepository repository,
            ILoverRepository loverRepository,
            ILoverCloudUserRepository userRepository,
            IPropertyMappingContainer propertyMappingContainer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
            _loverRepository = loverRepository;
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
            PaginatedList<LoverLog> loverLogs = 
                await _repository.GetLoverLogsAsync(userId, parameters);
            IEnumerable<LoverLogResource> loverLogResources = 
                _mapper.Map<IEnumerable<LoverLogResource>>(loverLogs)
                .AsQueryable()
                .ApplySort(
                    parameters.OrderBy, 
                    _propertyMappingContainer.Resolve<LoverLogResource, LoverLog>());
            IEnumerable<ExpandoObject> shapedLoverLogResources =
                loverLogResources.ToDynamicObject(parameters.Fields)
                .Select(x =>
                {
                    var dict = x as IDictionary<string, object>;
                    dict.Add(
                        "links", 
                        CreateLinksForLoverLog(
                            dict["Id"] as string, parameters.Fields));
                    return x;
                });

            var result = new
            {
                value = shapedLoverLogResources,
                links = CreateLinksForLoverLogs(parameters, loverLogs.HasPrevious, loverLogs.HasNext)
            };

            var metadata = new
            {
                loverLogs.PageIndex,
                loverLogs.PageSize,
                loverLogs.PageCount
            };
            Response.Headers.Add(
                LoverCloudApiConstraint.PaginationHeaderKey,
                new StringValues(
                    JsonConvert.SerializeObject(metadata, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })));

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
            ExpandoObject shapedLoverLogResource = loverLogResource.ToDynamicObject(fields);

            (shapedLoverLogResource as IDictionary<string, object>).Add("links", CreateLinksForLoverLog(id, fields));

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
            LoverCloudUser user = await _userRepository.FindByIdAsync(this.GetUserId());
            if (user.Lover == null) return UserNoLoverResult(user);
            Lover lover = user.Lover;

            LoverLog loverLog = _mapper.Map<LoverLog>(addResource);
            loverLog.Creater = user;
            loverLog.Lover = lover;
            loverLog.CreateDateTime = DateTime.Now;
            loverLog.LastUpdateTime = DateTime.Now;
            _repository.Add(loverLog);

            foreach(var formFile in addResource.Photos)
            {
                var photo = new LoverPhoto
                {
                    Name = formFile.FileName,
                    Uploader = user,
                    Lover = lover,
                    LoverLog = loverLog
                };
                photo.PhotoPhysicalPath = photo.GeneratePhotoPhysicalPath(formFile.GetFileSuffix());
                loverLog.LoverPhotos.Add(photo);
                await formFile.SaveToFileAsync(photo.PhotoPhysicalPath);
            }

            if (!await _unitOfWork.SaveChangesAsync()) return NoContent();

            LoverLogResource loverLogResource = _mapper.Map<LoverLogResource>(loverLog);
            ExpandoObject shapedLoverLogResource = loverLogResource.ToDynamicObject();
            (shapedLoverLogResource as IDictionary<string, object>).Add("links", CreateLinksForLoverLog(loverLog.Id));
            return CreatedAtRoute("AddLoverLog", loverLogResource);
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
            if (loverLog.CreaterId != this.GetUserId())
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

        private IActionResult UserNoLoverResult(LoverCloudUser user) => Forbid($"用户 {user} 还没有情侣, 无法操作资源");


        private IEnumerable<LinkResource> CreateLinksForLoverLog(string id, string fields = null)
        {
            return new LinkResource[]
            {
                new LinkResource("self", "get", Url.Link("GetLoverLog", new {id, fields})),
                new LinkResource("delete_log", "delete", Url.Link("DeleteLoverLog", new {id})),
                new LinkResource("update_log", "patch", Url.Link("PartiallyUpdateLoverLog", new{ id })),
            };
        }

        private IEnumerable<LinkResource> CreateLinksForLoverLogs(
            LoverLogParameters parameters, bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkResource>
            {
                new LinkResource("current_page", "get", Url.Link("GetLoverLogs", parameters))
            };

            if(hasPrevious)
            {
                parameters.PageIndex--;
                links.Add(
                    new LinkResource("previous_page", "get", Url.Link("GetLoverLogs", parameters)));
                parameters.PageIndex++;
            }
            if (hasNext)
            {
                parameters.PageIndex++;
                links.Add(
                    new LinkResource("next_page", "get", Url.Link("GetLoverLogs", parameters)));
                parameters.PageIndex--;
            }

            return links;
        }
    }
}
