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
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/menstruationLog")]
    [Authorize]
    public class MenstruationLogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMenstruationLogRepository _mlogRepository;
        private readonly ILoverCloudUserRepository _userRepository;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public MenstruationLogController(IUnitOfWork unitOfWork,
            IMapper mapper, 
            IMenstruationLogRepository mlogRepository,
            ILoverCloudUserRepository userRepository,
            IPropertyMappingContainer propertyMappingContainer)
        {
            _unitOfWork=unitOfWork;
            _mapper=mapper;
            _mlogRepository=mlogRepository;
            _userRepository=userRepository;
            _propertyMappingContainer=propertyMappingContainer;
        }

        [HttpPost(Name = "AddMenstruationLog")]
        public async Task<IActionResult> Add([FromBody]MenstruationLogAddResource addResource)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            LoverCloudUser user = await _userRepository.FindByIdAsync(this.GetUserId());
            if (user.Sex == Sex.Male) return BadRequest("该功能仅面向女性");
            if (user.Lover == null) return this.UserNoLoverResult(user);

            MenstruationLog mlog = _mapper.Map<MenstruationLog>(addResource);
            mlog.LoverCloudUser = user;
            _mlogRepository.Add(mlog);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to add menstruation log.");

            MenstruationLogResource mlogResource = _mapper.Map<MenstruationLogResource>(mlog);
            ExpandoObject result = mlogResource.ToDynamicObject()
                .AddLinks(this, null, "menstruation_log", null, 
                "DeleteMenstruationLog", "PartiallyUpdateMenstruationLog");

            return CreatedAtRoute("AddMenstruationLog", result);
        }

        [HttpDelete("{id}", Name = "DeleteMenstruationLog")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            string userId = this.GetUserId();
            LoverCloudUser user = await _userRepository.FindByIdAsync(userId);
            if (user.Sex == Sex.Male) return Forbid();

            MenstruationLog mlog = await _mlogRepository.FindByIdAsync(
                id, x => x.Include(l => l.LoverCloudUser)
                .Include(l => l.MenstruationDescriptions));
            if (mlog == null) return NotFound();
            if (mlog?.LoverCloudUser ?.Sex == Sex.Male || 
                mlog.LoverCloudUser?.Id != this.GetUserId())
                return Forbid();

            _mlogRepository.Delete(mlog);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to delete resource");

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateMenstruationLog")]
        public async Task<IActionResult> PartiallyUpdate(
            [FromRoute]string id, 
            [FromBody]JsonPatchDocument<MenstruationLogUpdateResource> patchDoc)
        {
            MenstruationLog mlog = await _mlogRepository.FindByIdAsync(
                id, 
                x=>x.Include(l => l.LoverCloudUser));
            if (mlog == null) return NotFound();
            if (mlog.LoverCloudUser?.Sex == Sex.Male || 
                mlog.LoverCloudUser?.Id != this.GetUserId())
                return Forbid();

            MenstruationLogUpdateResource mlogUpdateResource = _mapper.Map<MenstruationLogUpdateResource>(mlog);
            patchDoc.ApplyTo(mlogUpdateResource);
            _mapper.Map(mlogUpdateResource, mlog);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to update resource");

            return NoContent();
        }

        [HttpGet(Name = "GetMenstruationLogs")]
        public async Task<IActionResult> Get([FromQuery]MenstruationLogParameters parameters)
        {
            string userId = this.GetUserId();
            LoverCloudUser user = await _userRepository.FindByIdAsync(userId);
            if (user.Sex == Sex.Male) return Forbid();

            PaginatedList<MenstruationLog> mlogs = await _mlogRepository.GetAsync(userId, parameters);
            IEnumerable<MenstruationLog> sortedMlogs = mlogs.AsQueryable()
                .ApplySort(
                parameters.OrderBy, _propertyMappingContainer.Resolve<MenstruationLogResource, MenstruationLog>());

            IEnumerable<ExpandoObject> shapedMlogs = sortedMlogs.ToDynamicObject(parameters.Fields)
                .AddLinks(
                this, parameters.Fields, "menstruation_log", null, "DeleteMenstruationLog", "PartiallyUpdateMenstruationLog");

            var result = new
            {
                values = shapedMlogs,
                links = this.CreatePaginationLinks(
                    "GetMenstruationLogs", parameters, 
                    mlogs.HasPrevious, mlogs.HasNext),
            };

            this.AddPaginationHeaderToResponse(mlogs);

            return Ok(result);
        }
    }





    [ApiController]
    [Route("api/lovers/menstruationDescriptions")]
    [Authorize]
    public class MenstruationDescriptionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMenstruationDescriptionRepository _mdescriptionRepository;
        private readonly IMenstruationLogRepository _mlogRepository;
        private readonly ILoverCloudUserRepository _userRepository;

        public MenstruationDescriptionController(IUnitOfWork unitOfWork,
            IMapper mapper,
            IMenstruationDescriptionRepository mdescriptionRepository,
            IMenstruationLogRepository mlogRepository,
            ILoverCloudUserRepository userRepository)
        {
            _unitOfWork=unitOfWork;
            _mapper=mapper;
            _mdescriptionRepository=mdescriptionRepository;
            _mlogRepository=mlogRepository;
            _userRepository=userRepository;
        }

        /// <summary>
        /// 新建 姨妈描述 
        /// </summary>
        /// <param name="menstruationLogId">姨妈记录Id</param>
        /// <param name="addResource"></param>
        /// <returns></returns>
        [HttpPost("{menstruationLogId}", Name = "AddMenstruationDescription")]
        public async Task<IActionResult> Add(
            [FromRoute]string menstruationLogId, 
            [FromBody]MenstruationDescriptionAddResource addResource)
        {
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            MenstruationLog mlog = await _mlogRepository.FindByIdAsync(
                menstruationLogId, 
                c => c.Include(x => x.LoverCloudUser));
            if (mlog == null) return NotFound();
            if (mlog.LoverCloudUser?.Sex == Sex.Male || // 禁止男性用户
                mlog.LoverCloudUser?.Id != this.GetUserId()) 
                return Forbid();

            MenstruationDescription mDescription = _mapper.Map<MenstruationDescription>(addResource);
            mDescription.Date = DateTime.Now;
            mDescription.MenstruationLog = mlog;
            _mdescriptionRepository.Add(mDescription);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to add resource");

            MenstruationDescriptionResource mDescriptionResource = _mapper.Map<MenstruationDescriptionResource>(mDescription);

            var result = mDescriptionResource.ToDynamicObject()
                .AddLinks(
                this, null, "menstruation_description", null, 
                "DeleteMenstruationDescription", "PartiallyUpdateMenstruationDescription");

            return CreatedAtRoute("AddMenstruationDescription", result);
        }

        [HttpDelete("{id}", Name = "DeleteMenstruationDescription")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            MenstruationDescription mDescription = await _mdescriptionRepository
                .FindByIdAsync(
                id, q => q.Include(x => x.MenstruationLog).ThenInclude(x => x.LoverCloudUser));

            if (mDescription == null) return null;
            if (mDescription.MenstruationLog?.LoverCloudUser?.Sex == Sex.Male || // 禁止男性用户
                mDescription.MenstruationLog?.LoverCloudUser?.Id != this.GetUserId())
                return Forbid();

            _mdescriptionRepository.Delete(mDescription);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to delete resource");

            return NoContent();
        }

        [HttpPatch("{id}", Name = "PartiallyUpdateMenstruationDescription")]
        public async Task<IActionResult> PartiallyUpdate(
            [FromRoute]string id, 
            [FromBody]JsonPatchDocument<MenstruationDescriptionUpdateResource> patchDoc)
        {
            MenstruationDescription mDescription = await _mdescriptionRepository
                .FindByIdAsync(
                id, q => q.Include(x => x.MenstruationLog).ThenInclude(x => x.LoverCloudUser));

            if (mDescription == null) return null;
            if (mDescription.MenstruationLog?.LoverCloudUser?.Sex == Sex.Male || // 禁止男性用户
                mDescription.MenstruationLog?.LoverCloudUser?.Id != this.GetUserId())
                return Forbid();

            MenstruationDescriptionUpdateResource updateResource = _mapper.Map<MenstruationDescriptionUpdateResource>(mDescription);
            patchDoc.ApplyTo(updateResource);
            _mapper.Map(updateResource, mDescription);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to update resource");

            return NoContent();
        }

        [HttpGet("{menstruationLogId}", Name = "GetMenstruationDescriptions")]
        public async Task<IActionResult> Get(
            [FromRoute]string menstruationLogId,
            [FromQuery]MenstruationDescriptionParameters parameters)
        {
            LoverCloudUser user = await _userRepository.FindByIdAsync(
                this.GetUserId());

            if (user.Sex == Sex.Male)
                return Forbid();

            PaginatedList<MenstruationDescription> descriptions = 
                await _mdescriptionRepository.GetAsync(
                    user.Id, menstruationLogId, parameters);

            IEnumerable<MenstruationDescriptionResource> descriptionResources =
                _mapper.Map<IEnumerable<MenstruationDescriptionResource>>(descriptions);

            IEnumerable<ExpandoObject> shapedDescriptionResources =
                descriptionResources.ToDynamicObject(parameters.Fields);

            var result = new
            {
                value = shapedDescriptionResources,
                links = this.CreatePaginationLinks(
                    "GetMenstruationDescriptions", parameters,
                    descriptions.HasPrevious, descriptions.HasNext)
            };

            this.AddPaginationHeaderToResponse(descriptions);

            return Ok(result);
        }
    }
}
