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
    [Route("api/lovers/anniversaries")]
    internal class LoverAnniversaryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoverAnniversaryRepository _anniversaryRepository;
        private readonly IMapper _mapper;
        private readonly ILoverCloudUserRepository _userRepository;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public LoverAnniversaryController(IUnitOfWork unitOfWork,
            ILoverAnniversaryRepository anniversaryRepository,
            IMapper mapper,
            ILoverCloudUserRepository userRepository,
            IPropertyMappingContainer propertyMappingContainer)
        {
            _unitOfWork=unitOfWork;
            _anniversaryRepository=anniversaryRepository;
            _mapper=mapper;
            _userRepository=userRepository;
            _propertyMappingContainer=propertyMappingContainer;
        }

        /// <summary>
        /// 新建情侣纪念日
        /// </summary>
        /// <param name="addResource"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddLoverAnniversary")]
        public async Task<IActionResult> Add(
            [FromBody]LoverAnniversaryAddResource addResource)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            LoverCloudUser user = await _userRepository.FindByIdAsync(this.GetUserId());
            if (user.Lover == null) return this.UserNoLoverResult(user);

            LoverAnniversary anniversary = _mapper.Map<LoverAnniversary>(addResource);
            anniversary.Lover = user.Lover;
            _anniversaryRepository.Add(anniversary);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to add anniversary");

            ExpandoObject anniversaryResource = _mapper.Map<LoverAnniversaryResource>(anniversary)
                .ToDynamicObject();
            (anniversaryResource as IDictionary<string, object>).Add(
                "links", CreateLinksForAnniversary(anniversary.Id));

            return CreatedAtRoute("AddLoverAnniversary", anniversaryResource);
        }

        /// <summary>
        /// 删除情侣纪念日
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteLoverAnniversary")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            LoverAnniversary anniversary = await _anniversaryRepository.FindByIdAsync(id);
            if (anniversary == null) return NotFound();

            if (anniversary.Lover.HasUser(this.GetUserId()))
                return Forbid();

            _anniversaryRepository.Delete(anniversary);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to delete resource.");

            return NoContent();
        }

        /// <summary>
        /// 局部更新情侣纪念日
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PartiallyUpdateLoverAnniversary")]
        public async Task<IActionResult> PartiallyUpdate(
            [FromRoute]string id,
            [FromBody]JsonPatchDocument<LoverAnniversaryUpdateResource> patchDoc)
        {
            LoverAnniversary anniversary = await _anniversaryRepository.FindByIdAsync(id);

            if (anniversary == null) return NotFound();

            LoverAnniversaryUpdateResource anniversaryResource = _mapper.Map<LoverAnniversaryUpdateResource>(anniversary);
            patchDoc.ApplyTo(anniversaryResource);

            _mapper.Map(anniversaryResource, anniversary);

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]string id, [FromQuery]string fields)
        {
            LoverAnniversary anniversary =  await _anniversaryRepository.FindByIdAsync(id);
            if (anniversary == null) return NotFound();
            string userId = this.GetUserId();
            if (!(anniversary.Lover?.HasUser(userId) ?? false))
                return this.UserNoLoverResult(anniversary.Lover.GetUser(userId));

            LoverAnniversaryResource anniversaryResource = _mapper.Map<LoverAnniversaryResource>(anniversary);

            ExpandoObject shapedAnniversaryResource = anniversaryResource.ToDynamicObject(fields);

            shapedAnniversaryResource.TryAdd("links", CreateLinksForAnniversary(anniversary.Id, fields));

            return Ok(shapedAnniversaryResource);
        }
             
        /// <summary>
        /// 获取情侣纪念日
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet(Name = "GetLoverAnniversaries")]
        public async Task<IActionResult> Get([FromQuery]LoverAnniversaryParameters parameters)
        {
            PaginatedList<LoverAnniversary> anniversaries = 
                await _anniversaryRepository.GetAsync(this.GetUserId(), parameters);

            IQueryable<LoverAnniversary> sortedAnniversaries = anniversaries.AsQueryable()
                .ApplySort(
                    parameters.OrderBy,
                    _propertyMappingContainer.Resolve<LoverAnniversaryResource, LoverAnniversary>());

            IEnumerable<LoverAnniversaryResource> loverAnniversaryResources =
                _mapper.Map<IEnumerable<LoverAnniversaryResource>>(sortedAnniversaries);

            IEnumerable<ExpandoObject> shapedAnniversaryResource =
                loverAnniversaryResources.ToDynamicObject(parameters.Fields)
                .Select(x =>
                {
                    var dict = x as IDictionary<string, object>;
                    dict.Add("links", CreateLinksForAnniversary(dict["Id"] as string, parameters.Fields));
                    return x;
                });

            this.AddPaginationHeaderToResponse(anniversaries);

            var result = new
            {
                value = shapedAnniversaryResource,
                links = this.CreatePaginationLinks("GetLoverAnniversaries", parameters, anniversaries.HasPrevious, anniversaries.HasNext)
            };

            return Ok(result);
        }

        private object CreateLinksForAnniversaries(LoverAnniversaryParameters parameters, bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkResource>
            {
                new LinkResource("current_page", "get", Url.Link("GetAnniversaries", parameters))
            };

            if(hasPrevious)
            {
                parameters.PageIndex--;
                links.Add(
                new LinkResource(
                    "previous_page", "get", Url.Link("GetAnniversaries", parameters)));
                parameters.PageIndex++;
            }
            if (hasNext)
            {
                parameters.PageIndex++;
                links.Add(
                new LinkResource(
                    "next_page", "get", Url.Link("GetAnniversaries", parameters)));
                parameters.PageIndex--;
            }

            return links;
        }

        private IEnumerable<LinkResource> CreateLinksForAnniversary(string id, string fields = null)
        {
            return new LinkResource[]
            {
                new LinkResource(
                    "delete_anniversary", "delete",
                    Url.Link("DeleteLoverAnniversary", new { id })),
                new LinkResource(
                    "update_anniversary", "patch",
                    Url.Link("PartiallyUpdateLoverAnniversary", new { id })),
            };
        }
    }
}
