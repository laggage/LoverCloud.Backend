namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Api.Extensions;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Extensions;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/lovers")]
    [Authorize]
    public class LoverController : ControllerBase
    {
        private readonly UserManager<LoverCloudUser> _userManager;
        private readonly ILoverRepository _loverRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoverController(UserManager<LoverCloudUser> userManager,
            ILoverRepository loverRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _loverRepository = loverRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Api-LoverPhotos&&LoverAlbums

        /// <summary>
        /// 获取情侣照片
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>图片文件</returns>
        [HttpGet()]
        [Route("files/photos/{guid}", Name = "GetPhoto")]
        public async Task<IActionResult> GetLoverPhoto([FromRoute]string guid)
        {
            var loverPhoto = await _loverRepository.FindLoverPhotoByGuid(guid);
            return PhysicalFile(loverPhoto.PhotoPhysicalPath, $"image/png");
        }

        /// <summary>
        /// 获取情侣照片列表(支持翻页, 根据照片名搜索, 排序)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("photos", Name = "GetLoverPhotoResources")]
        public async Task<IActionResult> GetLoverPhotoResources([FromQuery]LoverPhotoParameters parameters)
        {
            PaginatedList<LoverPhoto> loverPhotos = 
                await _loverRepository.GetLoverPhotosAsync(GetLoverCloudUserId(), parameters);
            IEnumerable<LoverPhotoResource> loverPhotoResources = 
                _mapper.Map<IEnumerable<LoverPhotoResource>>(loverPhotos);
            IEnumerable<ExpandoObject> shapedLoverPhotoResources = 
                loverPhotoResources.ToDynamicObject(parameters.Fields);
            var result = new
            {
                value = shapedLoverPhotoResources,
                links = CreateLinksForLoverPhotos(
                    parameters, loverPhotos.HasPrevious, loverPhotos.HasNext)
            };

            return Ok(result);
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

        /// <summary>
        /// 上传照片资源
        /// </summary>
        /// <param name="formCollection">表单集合, 一个照片文件对应一个loverPhoto</param>
        /// <returns></returns>
        [HttpPost()]
        [Route("photos", Name = "AddLoverPhoto")]
        public async Task<IActionResult> AddLoverPhoto([FromForm]IFormCollection formCollection)
        {
            formCollection.TryGetValue("loverPhoto", out StringValues stringValues);
            if (stringValues.Count != formCollection.Files.Count) return BadRequest();
            
            var lover = await _loverRepository.GetLoverByLoverCloudUserIdAsync(GetLoverCloudUserId());
            var user = lover.LoverCloudUsers.FirstOrDefault(x => x.Id == GetLoverCloudUserId());
            if (user == null) return Unauthorized();
            var loverPhotos = new List<LoverPhoto>();
            for (int i = 0; i< stringValues.Count;i++)
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
                loverPhoto.PhotoUrl = Url.Link("GetPhoto", new { loverPhoto.Guid });
                
                _loverRepository.AddLoverPhoto(loverPhoto);
                loverPhotos.Add(loverPhoto);
                await formFile.SaveToFileAsync(photoPath);
            }

            bool result = await _unitOfWork.SaveChangesAsync();
            if(!result) loverPhotos.DeletePhysicalFiles();

            var loverPhotoResource = _mapper.Map<IEnumerable<LoverPhotoResource>>(loverPhotos);
            return CreatedAtRoute("AddLoverPhoto",null, loverPhotoResource);
        }

        #endregion

        #region Api-LoverLog

        /// <summary>
        /// 情侣日志Api接口
        /// </summary>
        /// <returns>情侣日志</returns>
        [HttpGet("logs")]
        public async Task<IActionResult> GetLoverLogs()
        {
            var loverCloudUserId = GetLoverCloudUserId();
            var result = await _loverRepository.GetLoverLogsByLoverCloudUserIdAsync(loverCloudUserId);
            return Ok(result);
        }

        [HttpPost("logs", Name = "CreateLoverLog")]
        public async Task<IActionResult> CreateLoverLog([FromBody]LoverLogAddResource addResource)
        {
            var lover = await _loverRepository.GetLoverByLoverCloudUserIdAsync(GetLoverCloudUserId());
            if (lover == null) return NoContent();

            var loverLog = _mapper.Map<LoverLog>(addResource);
            loverLog.Lover = lover;
            loverLog.CreateDateTime = DateTime.Now;
            loverLog.LastUpdateTime = DateTime.Now;
            if (loverLog == null)
                return NoContent();
            await _loverRepository.AddLoverLogAsync(loverLog);
            var res = await _unitOfWork.SaveChangesAsync();
            if (!res) return NoContent();
            var loverLogResource = _mapper.Map<LoverLogResource>(loverLog);
            return CreatedAtRoute("CreateLoverLog", new {loverLog.Guid}, loverLogResource);
        }

        #endregion


        #region Api-LoverRequest

        [HttpPost("loverrequest", Name = "AddLoverRequest")]
        public async Task<IActionResult> AddLoverRequest(LoverRequestAddResource addResource)
        {
            var requester = await GetLoverCloudUserAsync();
            if (requester == null) return Unauthorized();
            var receiver = await _userManager.FindByIdAsync(addResource.ReceiverGuid);
            if (receiver == null) return BadRequest($"找不到Guid为 {addResource.ReceiverGuid} 的用户");

            if (receiver.Lover != null)
                return BadRequest("对方已有情侣, 无法发出请求.");
            
            var loverRequest = _mapper.Map<LoverRequest>(addResource);
            loverRequest.Receiver = receiver;
            loverRequest.Requester = requester;
            loverRequest.Succeed = null;
            loverRequest.RequestDate = DateTime.Now;

            await _loverRepository.AddLoverRequestAsync(loverRequest);
            var result = await _unitOfWork.SaveChangesAsync();
            if (!result) throw new Exception();
            var loverRequestResource = _mapper.Map<LoverRequestResource>(loverRequest);

            return CreatedAtRoute("AddLoverRequest", new { loverRequest.Guid }, loverRequestResource);
        }

        [HttpPatch("loverrequest/{guid}")]
        public async Task<IActionResult> PatchLoverRequest([FromRoute]string guid,[FromBody] JsonPatchDocument<LoverRequestUpdateResource> loverRequestPatchDocument)
        {
            if (string.IsNullOrEmpty(guid)) return BadRequest();

            var loverRequestToUpdate = await _loverRepository.GetLoverRequestByGuidAsync(guid);
            if (loverRequestToUpdate == null) return BadRequest($"找不到对应的 LoverRequest({guid})");
            string userId = GetLoverCloudUserId();
            if (!(loverRequestToUpdate.ReceiverGuid == userId)) 
                return BadRequest("非法用户, 无权操作");

            var loverRequestToUpdateResource = _mapper.Map<LoverRequestUpdateResource>(loverRequestToUpdate);
            loverRequestPatchDocument.ApplyTo(loverRequestToUpdateResource);
            _mapper.Map(loverRequestToUpdateResource, loverRequestToUpdate);

            if (loverRequestToUpdateResource.Succeed == true &&
                loverRequestToUpdate.LoverGuid == null &&
                loverRequestToUpdate.Receiver.Lover == null &&
                loverRequestToUpdate.Requester.Lover == null)
            {
                var users = new LoverCloudUser[]
                {
                    loverRequestToUpdate.Receiver,
                    loverRequestToUpdate.Requester
                };

                var lover = new Lover
                {
                    
                    RegisterDate = DateTime.Now,
                    LoverCloudUsers = users
                };
                loverRequestToUpdate.Lover = lover;
                await _loverRepository.AddLoverAsync(lover);
            }
            
            if (!await _unitOfWork.SaveChangesAsync()) throw new Exception("保存数据到数据库失败");
            
            return NoContent();
        }

        #endregion

        /// <summary>
        /// 通过 HttpContext 中的 User 属性获得对应的 <see cref="LoverCloudUser"/> 
        /// </summary>
        /// <returns> <see cref="LoverCloudUser"/> </returns>
        private Task<LoverCloudUser> GetLoverCloudUserAsync() => 
             _userManager.FindByIdAsync(GetLoverCloudUserId());

        /// <summary>
        /// 获取通过身份认证的用户的Id
        /// </summary>
        /// <returns> 经过身份认证的用户的Id </returns>
        private string GetLoverCloudUserId() =>
            User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
