namespace LoverCloud.Api.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Resources;
    using LoverCloud.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using System.Collections.Generic;
    using Microsoft.Extensions.Primitives;
    using Microsoft.AspNetCore.Http;
    using LoverCloud.Api.Extensions;
    using Newtonsoft.Json;
    using LoverCloud.Infrastructure.Extensions;
    using Newtonsoft.Json.Serialization;
    using System.Linq;
    using System.Dynamic;
    using LoverCloud.Core.Extensions;

    [ApiController]
    [Authorize]
    [Route("api/lovers/photos")]
    public class LoverPhotoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingContainer _propertyMappingContainer;
        private readonly ILoverPhotoRepository _repository;
        private readonly ILoverRepository _loverRepository;

        public LoverPhotoController(IUnitOfWork unitOfWork,
            IMapper mapper,
            IPropertyMappingContainer propertyMappingContainer,
            ILoverPhotoRepository repository,
            ILoverRepository loverRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _propertyMappingContainer = propertyMappingContainer;
            _repository = repository;
            _loverRepository = loverRepository;
        }

        /// <summary>
        /// 获取情侣照片
        /// </summary>
        /// <param name="id"></param>
        /// <returns>图片文件</returns>
        [HttpGet("files/{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetLoverPhoto([FromRoute]string id)
        {
            var loverPhoto = await _repository.FindByIdAsync(id);
            return PhysicalFile(loverPhoto.PhotoPhysicalPath, $"image/png");
        }

        /// <summary>
        /// 上传照片资源
        /// </summary>
        /// <param name="formCollection">表单集合, 一个照片文件对应一个loverPhoto</param>
        /// <returns></returns>
        [HttpPost(Name = "AddLoverPhoto")]
        public async Task<IActionResult> AddLoverPhoto([FromForm]IFormCollection formCollection)
        {
            formCollection.TryGetValue("loverPhoto", out StringValues stringValues);
            if (stringValues.Count != formCollection.Files.Count) return BadRequest();

            var lover = await _loverRepository.FindByUserIdAsync(this.GetUserId());
            var user = lover.LoverCloudUsers.FirstOrDefault(x => x.Id == this.GetUserId());
            if (user == null) return Unauthorized();
            var loverPhotos = new List<LoverPhoto>();
            for (int i = 0; i < stringValues.Count; i++)
            {   // 构造LoverPhoto, 并保存上传的图片文件
                string stringValue = stringValues[i];
                var formFile = formCollection.Files[i];
                string fileSuffix = formFile.GetFileSuffix();
                if (string.IsNullOrEmpty(fileSuffix)) return BadRequest("无法解析文件名");

                LoverPhotoAddResource loverPhotoAddResource = JsonConvert.DeserializeObject<LoverPhotoAddResource>(stringValue);
                var loverPhoto = _mapper.Map<LoverPhoto>(loverPhotoAddResource);
                loverPhoto.Uploader = user;
                loverPhoto.Lover = lover;

                string photoPath = loverPhoto.GeneratePhotoPhysicalPath(fileSuffix);

                loverPhoto.PhotoPhysicalPath = photoPath;
                loverPhoto.PhotoUrl = Url.Link("GetPhoto", new { loverPhoto.Id });

                _repository.Add(loverPhoto);
                loverPhotos.Add(loverPhoto);
                await formFile.SaveToFileAsync(photoPath);
            }

            bool result = await _unitOfWork.SaveChangesAsync();
            if (!result) loverPhotos.DeletePhysicalFiles(); // 回退操作

            var loverPhotoResource = _mapper.Map<IEnumerable<LoverPhotoResource>>(loverPhotos);
            return CreatedAtRoute("AddLoverPhoto", null, loverPhotoResource);
        }

        /// <summary>
        /// 获取情侣照片列表(支持翻页, 根据照片名搜索, 排序)
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetLoverPhotoResources")]
        public async Task<IActionResult> GetLoverPhotoResources([FromQuery]LoverPhotoParameters parameters)
        {
            PaginatedList<LoverPhoto> loverPhotos =
                await _repository.GetLoverPhotosAsync(this.GetUserId(), parameters);
            var propertyMapping = _propertyMappingContainer.Resolve<LoverPhotoResource, LoverPhoto>();
            IEnumerable<LoverPhotoResource> loverPhotoResources =
                _mapper.Map<IQueryable<LoverPhotoResource>>(loverPhotos)
                    .ApplySort(parameters.OrderBy, propertyMapping);
            IEnumerable<ExpandoObject> shapedLoverPhotoResources =
                loverPhotoResources.ToDynamicObject(parameters.Fields);
            var metadata = new
            {
                PageIndex = loverPhotos.PageIndex,
                PageSize = loverPhotos.PageSize,
                PageCount = loverPhotos.PageCount
            };
            var result = new
            {
                value = shapedLoverPhotoResources,
                links = CreateLinksForLoverPhotos(
                    parameters, loverPhotos.HasPrevious, loverPhotos.HasNext)
            };

            Response.Headers.Add(
                LoverCloudApiConstraint.PaginationHeaderKey,
                JsonConvert.SerializeObject(metadata, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoverPhotoResource([FromRoute]string id)
        {
            var loverPhoto = await _repository.FindByIdAsync(id);

            loverPhoto.DeletePhyicalFile();

            return NoContent();
        }

        private IEnumerable<LinkResource> CreateLinksForLoverPhotos(
            QueryParameters parameters, bool hasPreviouss, bool hasNext)
        {
            string routeName = "GetLoverPhotoResources";
            var newParameters = new LoverPhotoParameters { PageIndex = parameters.PageIndex, PageSize = parameters.PageSize };

            var linkResources = new List<LinkResource>
            {
                new LinkResource(
                    "self", "get", Url.Link(routeName, newParameters)),
            };
            if (hasPreviouss)
            {
                newParameters.PageIndex -= 1;
                linkResources.Add(
                    new LinkResource(
                        "previous_page", "get", Url.Link(routeName, newParameters)));
            }

            if (hasNext)
            {
                newParameters.PageIndex += 1;
                linkResources.Add(
                    new LinkResource(
                        "next_page", "get", Url.Link(routeName, newParameters)));
            }

            return linkResources;
        }
    }
}
