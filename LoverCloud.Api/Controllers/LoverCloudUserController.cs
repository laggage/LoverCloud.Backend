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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class LoverCloudUserController : ControllerBase
    {
        private readonly UserManager<LoverCloudUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILoverCloudUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoverLogRepository _logRepository;
        private readonly ILoverAnniversaryRepository _anniversaryRepository;
        private readonly ILoverAlbumRepository _albumRepository;

        public LoverCloudUserController(
            UserManager<LoverCloudUser> userManager,
            IMapper mapper,
            ILoverCloudUserRepository repository,
            IUnitOfWork unitOfWork,
            ILoverLogRepository logRepository,
            ILoverAnniversaryRepository anniversaryRepository,
            ILoverAlbumRepository albumRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logRepository=logRepository;
            _anniversaryRepository=anniversaryRepository;
            _albumRepository=albumRepository;
        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        [HttpGet("profileImage/{userId}", Name = "GetProfileImage")]
        public async Task<IActionResult> GetProfileImage([FromRoute]string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("user id not assigned");
            //string loggedUserId = this.GetUserId();
            LoverCloudUser user = await _repository.FindByIdAsync(userId);

            if (user.Lover == null) return this.UserNoLoverResult(user);
            // 只允许本人或其情侣获取自己或对方的头像...
            // if (userId != loggedUserId && user.GetSpouse()?.Id != loggedUserId) return Forbid();

            if (user == null) return NotFound($"找不到id为 {userId} 的用户");

            if (string.IsNullOrEmpty(user.ProfileImagePhysicalPath) ||
                !System.IO.File.Exists(user.ProfileImagePhysicalPath))
                return NotFound($"用户 {user} 还没有上传头像");

            return PhysicalFile(user.ProfileImagePhysicalPath, "image/png");
        }

        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <returns>用户上传的图片</returns>
        [HttpPost("profileImage", Name = "UploadProfileImage")]
        public async Task<IActionResult> PostProfileImage([FromForm]IFormFile profileImage)
        {
            LoverCloudUser user = await _repository.FindByIdAsync(this.GetUserId());
            if (user == null) return BadRequest("无法获得用户信息");
            await SaveProfileImageAsync(profileImage, user);
            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("保存数据失败");
            return PhysicalFile(user.ProfileImagePhysicalPath, "image/png");
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="addResource"> <see cref="LoverCloudUserAddResource"/> </param>
        /// <returns> <see cref="LoverCloudUserResource"/> </returns>
        [HttpPost(Name = "Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm]LoverCloudUserAddResource addResource)
        {
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            var user = _mapper.Map<LoverCloudUser>(addResource);
            await SaveProfileImageAsync(addResource.ProfileImage, user);

            if (user == null) return NoContent();

            user.RegisterDate = DateTime.Now;
            var res = await _userManager.CreateAsync(user, addResource.Password);
            if (!res.Succeeded) return NoContent();

            var loverCloudUserResource = _mapper.Map<LoverCloudUserResource>(user);

            return CreatedAtRoute("Register",new { user.Id } , loverCloudUserResource);
        }

        private async Task SaveProfileImageAsync(IFormFile profile, LoverCloudUser user)
        {
            if (profile == null || user == null) return;
            // 完整的文件保存路径
            var physicalPath = user.GenerateProfileImagePhysicalPath(
                profile.GetFileSuffix());
            // 删除之前的头像
            var dir = Path.GetDirectoryName(physicalPath);
            if(Directory.Exists(dir))
                foreach (string file in Directory.EnumerateFiles(dir))
                    System.IO.File.Delete(file);

            // 保存上传的文件
            await profile.SaveToFileAsync(physicalPath);
            user.ProfileImagePhysicalPath = physicalPath;
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string fields)
        {
            string id = this.GetUserId();
            if (string.IsNullOrEmpty(id)) return Unauthorized();

            var user = await _repository.FindByIdAsync(id);
            var userResource = _mapper.Map<LoverCloudUserResource>(user);
            if(userResource.Spouse != null)
            {
                userResource.Spouse.Spouse = null;
                userResource.Spouse.ReceivedLoverRequests = null;
            }

            if (userResource.LoverRequests != null && userResource.LoverRequests.Count > 0)
            {
                foreach (var loverRequest in userResource.LoverRequests)
                {
                    loverRequest.Requester = null;
                    if (userResource.Spouse != null)
                        loverRequest.Receiver = null;
                }
            }
            if(userResource.ReceivedLoverRequests != null && userResource.ReceivedLoverRequests.Count > 0)
            {
                userResource.ReceivedLoverRequests = userResource.ReceivedLoverRequests.Select(s =>
                {
                    s.Requester = null;
                    return s;
                }).ToList();
            }

            userResource.ProfileImageUrl = Url.Link("GetProfileImage", new { userId = userResource.Id });
            if(userResource.Spouse != null)
            {
                userResource.Spouse.ProfileImageUrl = Url.Link("GetProfileImage", new { userId = userResource.Spouse.Id });
                userResource.Spouse.LoverAlbumCount = userResource.LoverAlbumCount = await _albumRepository.CountAsync(user.Lover.Id);
                userResource.Spouse.LoverLogCount = userResource.LoverLogCount = await _logRepository.CountAsync(user.Lover.Id);
                userResource.Spouse.LoverAnniversaryCount = userResource.LoverAnniversaryCount = await _anniversaryRepository.CountAsync(user.Lover.Id);
            }

            var shapedUserResource = userResource.ToDynamicObject(fields.Contains("spouse,") || string.IsNullOrEmpty(fields) ? fields.Replace("spouse,", ""):fields);
            if (userResource.Spouse != null)
                (shapedUserResource as IDictionary<string, object>).Add("spouse", userResource.Spouse.ToDynamicObject(
                $"sex, userName, birth, id, profileImageUrl"));

            return Ok(shapedUserResource);
        }

        /// <summary>
        /// 用户信息修改
        /// </summary>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> Patch(
            [FromBody]JsonPatchDocument<LoverCloudUserUpdateResource> patchDoc)
        {
            LoverCloudUser user = await _repository.FindByIdAsync(this.GetUserId());

            if (user == null)
                return BadRequest("无法获得用户信息");

            LoverCloudUserUpdateResource userUpdateResource = 
                _mapper.Map<LoverCloudUserUpdateResource>(user);

            patchDoc.ApplyTo(userUpdateResource);
            _mapper.Map(userUpdateResource, user);

            if (!await _unitOfWork.SaveChangesAsync())
                throw new Exception("保存数据失败");

            return NoContent();
        }
    }
}
