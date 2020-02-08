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
    using System.Linq;
    using System.Dynamic;
    using Microsoft.AspNetCore.JsonPatch;
    using System;

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
        private readonly ILoverCloudUserRepository _userRepository;

        public LoverPhotoController(IUnitOfWork unitOfWork,
            IMapper mapper,
            IPropertyMappingContainer propertyMappingContainer,
            ILoverPhotoRepository repository,
            ILoverRepository loverRepository,
            ILoverCloudUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _propertyMappingContainer = propertyMappingContainer;
            _repository = repository;
            _loverRepository = loverRepository;
            _userRepository=userRepository;
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

        [HttpPost(Name = "AddLoverPhoto")]
        public async Task<IActionResult> Add([FromForm]LoverPhotoAddResource loverPhotoAddResource)
        {
            IFormFile file = loverPhotoAddResource.File;
            if (file == null)   // 表单中必须包含图片文件
            {
                ModelState.AddModelError("loverPhotoAddResource", $"parameter {file} cannot be null");
                return BadRequest(ModelState);
            }

            // 无法自动映射表单的Tags到对应的Tags集合属性, 所以手动处理一下, 读取key为Tags的值, 反序列化json
            // 元数据是json数组, 示例: [{"name": "value"}, {"name", "value2"}] 
            // 表单中只能有一个tags键
            Request.Form.TryGetValue("tags", out StringValues tagsStrings);
            if (tagsStrings.Count > 1) return BadRequest();
            IList<TagAddResource> tags =
                JsonConvert.DeserializeObject<IList<TagAddResource>>(tagsStrings.FirstOrDefault());

            loverPhotoAddResource.Tags = tags;

            LoverCloudUser user = await _userRepository.FindByIdAsync(this.GetUserId());

            Lover lover = user.Lover;

            LoverPhoto loverPhoto = _mapper.Map<LoverPhoto>(loverPhotoAddResource);
            // 生成 PhotoPhysicalPath 要用到 Uploader, 所以先设置 Uploader 的值
            loverPhoto.Uploader = user;
            loverPhoto.Lover = lover;
            loverPhoto.PhotoPhysicalPath = loverPhoto.GeneratePhotoPhysicalPath(file.GetFileSuffix()); ;
            loverPhoto.UpdateDate = DateTime.Now;
            loverPhoto.PhotoUrl = Url.Link("GetPhoto", new {id = loverPhoto.Id});
            // 添加到数据库
            _repository.Add(loverPhoto);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("数据保存失败");
            // 保存图片文件
            await file.SaveToFileAsync(loverPhoto.PhotoPhysicalPath);

            LoverPhotoResource loverPhotoResource = _mapper.Map<LoverPhotoResource>(loverPhoto);
            ExpandoObject result = loverPhotoResource.ToDynamicObject()
                .AddLinks(this, null, "photo", "GetPhoto", "DeleteLoverPhoto", "PartiallyUpdateLoverPhoto");

            return CreatedAtRoute("AddLoverPhoto", result);
        }

        /// <summary>
        /// 获取情侣照片列表(支持翻页, 根据照片名搜索, 排序)
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetLoverPhotoResources")]
        public async Task<IActionResult> Get([FromQuery]LoverPhotoParameters parameters)
        {
            PaginatedList<LoverPhoto> photos =
                await _repository.GetLoverPhotosAsync(this.GetUserId(), parameters);

            IQueryable<LoverPhoto> sortedPhotos = photos.AsQueryable()
                .ApplySort(
                parameters.OrderBy,
                _propertyMappingContainer.Resolve<LoverPhotoResource, LoverPhoto>());

            IEnumerable<LoverPhotoResource> loverPhotoResources =
                _mapper.Map<IEnumerable<LoverPhotoResource>>(sortedPhotos);

            IEnumerable<ExpandoObject> shapedLoverPhotoResources =
                loverPhotoResources.ToDynamicObject(parameters.Fields)
                .AddLinks(this, parameters.Fields, "photo", "GetPhoto", "DeleteLoverPhoto", "PartiallyUpdateLoverPhoto");

            this.AddPaginationHeaderToResponse(photos);

            var result = new
            {
                value = shapedLoverPhotoResources,
                links = this.CreatePaginationLinks(
                    "GetLoverPhotoResources", parameters, 
                    photos.HasPrevious, photos.HasNext)
            };
            
            return Ok(result);
        }

        [HttpDelete("{id}", Name = "DeleteLoverPhoto")]
        public async Task<IActionResult> Delete([FromRoute]string id)
        {
            var loverPhoto = await _repository.FindByIdAsync(id);
            if (loverPhoto == null) return NotFound();
            _repository.Delete(loverPhoto);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("数据库保存失败");

            loverPhoto.DeletePhyicalFile();

            return NoContent();
        }

        /// <summary>
        /// 更新 情侣照片(<see cref="LoverPhoto"/>) 信息
        /// </summary>
        /// <param name="id">情侣照片id</param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PartiallyUpdateLoverPhoto")]
        public async Task<IActionResult> PartiallyUpdate(
            [FromRoute]string id, [FromBody]JsonPatchDocument patchDoc)
        {
            LoverPhoto loverPhotoToUpdate = await _repository.FindByIdAsync(id);
            if (loverPhotoToUpdate == null) return NotFound();

            LoverPhotoUpdateResource loverPhotoUpdateResource = _mapper.Map<LoverPhotoUpdateResource>(loverPhotoToUpdate);
            patchDoc.ApplyTo(loverPhotoUpdateResource);
            _mapper.Map(loverPhotoUpdateResource, loverPhotoToUpdate);

            if(!await _unitOfWork.SaveChangesAsync())
                throw new Exception("保存数据失败");
            return NoContent();
        }
    }
}
