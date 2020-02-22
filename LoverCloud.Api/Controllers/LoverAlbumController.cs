namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Api.Authorizations;
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
    [Route("api/lovers/albums")]
    [Authorize(Policy = AuthorizationPolicies.LoverResourcePolicy)]
    public class LoverAlbumController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoverAlbumRepository _albumRepository;
        private readonly ILoverCloudUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingContainer _propertyMappingContainer;
        private readonly IAuthorizationService _authorizationService;

        public LoverAlbumController(IUnitOfWork unitOfWork, 
            ILoverAlbumRepository albumRepository,
            ILoverCloudUserRepository userRepository,
            IMapper mapper, 
            IPropertyMappingContainer propertyMappingContainer,
            IAuthorizationService authorizationService)
        {
            _unitOfWork=unitOfWork;
            _albumRepository=albumRepository;
            _userRepository=userRepository;
            _mapper=mapper;
            _propertyMappingContainer=propertyMappingContainer;
            _authorizationService=authorizationService;
        }


        [HttpGet("{id}", Name = "GetLoverAlbum")]
        public async Task<IActionResult> Get([FromRoute]string id, [FromQuery]string fields)
        {
            LoverAlbum album = await _albumRepository.FindByIdAsync(id);
            // 确保该资源属于当前登录的用户
            if (!album?.Lover.LoverCloudUsers.Any(user => user.Id == this.GetUserId()) ?? false)
                return Forbid();
            if (album == null) return NotFound();

            LoverAlbumResource albumResource = _mapper.Map<LoverAlbumResource>(album);
            albumResource.PhotosCount = await _albumRepository.GetPhotosCount(albumResource.Id);
            ExpandoObject shapedAlbumResource = albumResource.ToDynamicObject(fields)
                .AddLinks(this, fields, "album", "GetLoverAlbum", "DeleteLoverAlbum", "PartiallyUpdateLoverAlbum");

            return Ok(shapedAlbumResource);
        }

        [HttpGet(Name = "GetLoverAlbums")]
        public async Task<IActionResult> Get([FromQuery]LoverAlbumParameters parameters)
        {
            PaginatedList<LoverAlbum> albums = await _albumRepository.GetLoverAlbumsAsync(this.GetUserId(), parameters);

            IQueryable<LoverAlbum> sortedAlbums = albums.AsQueryable()
                .ApplySort(
                parameters.OrderBy,
                _propertyMappingContainer.Resolve<LoverAlbumResource, LoverAlbum>());

            IEnumerable<LoverAlbumResource> albumResources = 
                _mapper.Map<IEnumerable<LoverAlbumResource>>(sortedAlbums);

            foreach (LoverAlbumResource source in albumResources)
            {
                source.PhotosCount = await _albumRepository.GetPhotosCount(source.Id);
                LoverPhoto converImage = await _albumRepository.GetCoverImage(source.Id);
                if (converImage != null)
                    source.CoverImageUrl = Url.Link("GetPhoto", new { id = converImage.Id });
            }

            IEnumerable<ExpandoObject> shapedAlbumResources = albumResources
                .ToDynamicObject(parameters.Fields)
                .AddLinks(
                this, parameters.Fields, "album",
                "GetLoverAlbum", "DeleteLoverAlbum", "PartiallyUpdateLoverAlbum");

            var result = new
            {
                value = shapedAlbumResources,
                links = this.CreatePaginationLinks("GetLoverAlbums", parameters, albums.HasPrevious, albums.HasNext)
            };

            this.AddPaginationHeaderToResponse(albums);

            return Ok(result);
        }

        /// <summary>
        /// 新建相册
        /// </summary>
        /// <param name="addResource"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddLoverAlbum")]
        public async Task<IActionResult> Add([FromBody]LoverAlbumAddResource addResource)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            LoverCloudUser user = await GetUserAsync();
            //if (user.Lover == null) return UserNoLoverResult(user);

            LoverAlbum album = _mapper.Map<LoverAlbum>(addResource);
            album.Creater = user;
            album.Lover = user.Lover;

            _albumRepository.Add(album);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("Failed to save changes");

            LoverAlbumResource albumResource = _mapper.Map<LoverAlbumResource>(album);

            ExpandoObject shapedAlbumResource = albumResource.ToDynamicObject()
                .AddLinks(this, null, "album", "GetLoverAlbum", "DeleteLoverAlbum", "PartiallyUpdateLoverAlbum");

            return CreatedAtRoute("AddLoverAlbum", shapedAlbumResource);
        }

        /// <summary>
        /// 删除相册
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteLoverAlbum")]
        public async Task<IActionResult> Delete(string id)
        {
            LoverAlbum albumToDelete = await _albumRepository.FindByIdAsync(id);

            if (albumToDelete == null) return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, albumToDelete, Operations.Delete);
            if(!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            _albumRepository.Delete(albumToDelete);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception($"Failed to delete album {id}, {albumToDelete.Name}");

            return NoContent();
        }

        /// <summary>
        /// 更新相册信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = "PartiallyUpdateLoverAlbum")]
        public async Task<IActionResult> PartiallyUpdate(
            [FromRoute]string id,  [FromBody]JsonPatchDocument<LoverAlbumUpdateResource> patchDoc)
        {
            LoverAlbum album = await _albumRepository.FindByIdAsync(id);
            if (album == null) return NotFound();

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, album, Operations.Update);
            if (!authorizationResult.Succeeded) return Forbid();

            LoverAlbumUpdateResource loverAlbumUpdateResource = _mapper.Map<LoverAlbumUpdateResource>(album);
            patchDoc.ApplyTo(loverAlbumUpdateResource);
            _mapper.Map(loverAlbumUpdateResource, album);
            album.LastUpdate = DateTime.Now;
            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception($"Failed to update album, id: {album.Id}");

            return NoContent();
        }

        private  Task<LoverCloudUser> GetUserAsync()
        {
            return _userRepository.FindByIdAsync(this.GetUserId());
        }
    }
}
