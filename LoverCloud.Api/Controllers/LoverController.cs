﻿namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
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
    using System.IO;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [ApiController()]
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

        [HttpGet()]
        [Route("files/photos/{guid}", Name = "GetPhoto")]
        public IActionResult GetLoverPhoto([FromRoute]string guid)
        {
            return PhysicalFile(
                Path.Combine(Directory.GetCurrentDirectory(), "1.jpg"), "image/png");
        }

        [HttpGet]
        [Route("photos")]
        public IActionResult GetLoverPhotoResources()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 上传照片资源
        /// </summary>
        /// <param name="formCollection">表单集合, 一个照片文件对应一个loverPhoto</param>
        /// <returns></returns>
        [HttpPost(Name = "AddLoverPhoto")]
        [Route("photos")]
        public async Task<IActionResult> AddLoverPhoto([FromForm]IFormCollection formCollection)
        {
            formCollection.TryGetValue("loverPhoto", out StringValues stringValues);
            if (stringValues.Count != formCollection.Files.Count) return BadRequest();
            
            var lover = await _loverRepository.GetLoverByLoverCloudUserIdAsync(GetLoverCloudUserId());
            var user = lover.LoverCloudUsers.FirstOrDefault(x => x.Id == GetLoverCloudUserId());
            var loverPhotos = new List<LoverPhoto>();
            if (user == null) return Unauthorized();
            for(int i = 0; i< stringValues.Count;i++)
            {
                var stringValue = stringValues[i];
                var formFile = formCollection.Files[i];
                string fileSuffix = formFile.FileName.Split('.').LastOrDefault();
                if (string.IsNullOrEmpty(fileSuffix)) return BadRequest("无法解析文件名");

                var loverPhotoAddResource = JsonConvert.DeserializeObject<LoverPhotoAddResource>(stringValue);
                var loverPhoto = _mapper.Map<LoverPhoto>(loverPhotoAddResource);
                loverPhoto.Uploader = user;
                loverPhoto.Lover = lover;

                string directoryToSave = loverPhoto.GeneratePhotoSaveDirectory();
                Directory.CreateDirectory(directoryToSave);
                string photoPath = Path.Combine(directoryToSave, $"{loverPhoto.Guid}.{fileSuffix}");
                
                loverPhoto.PhotoPhysicalPath = photoPath;
                loverPhoto.PhotoUrl = Url.Link("GetPhoto", new { loverPhoto.Guid });

                using var fs = new FileStream(photoPath, FileMode.OpenOrCreate);
                await formFile.CopyToAsync(fs);
                fs.Close();
                _loverRepository.AddLoverPhoto(loverPhoto);
                loverPhotos.Add(loverPhoto);
            }

            bool result = await _unitOfWork.SaveChangesAsync();
            if (!result)
            {
                foreach (var loverPhoto in loverPhotos)
                {
                    if (System.IO.File.Exists(loverPhoto.PhotoPhysicalPath))
                        System.IO.File.Delete(loverPhoto.PhotoPhysicalPath);
                }
                throw new InvalidDataException("保存数据到数据库失败");
            }
            return Ok(Request.Form.Files);
            throw new NotImplementedException();
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
