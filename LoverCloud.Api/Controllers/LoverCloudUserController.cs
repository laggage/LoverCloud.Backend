namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Extensions;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/users")]
    public class LoverCloudUserController : ControllerBase
    {
        private readonly UserManager<LoverCloudUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILoverCloudUserRepository _repository;

        public LoverCloudUserController(UserManager<LoverCloudUser> userManager,
            IMapper mapper,
            ILoverCloudUserRepository repository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="addResource">用户注册信息</param>
        /// <returns></returns>
        [HttpPost(Name = "Register")]
        public async Task<IActionResult> Register([FromBody]LoverCloudUserAddResource addResource)
        {
            var loverCloudUser = _mapper.Map<LoverCloudUser>(addResource);

            if (loverCloudUser == null) return NoContent();

            loverCloudUser.RegisterDate = DateTime.Now;
            var res = await _userManager.CreateAsync(loverCloudUser, addResource.Password);
            if (!res.Succeeded) return NoContent();

            var loverCloudUserResource = _mapper.Map<LoverCloudUserResource>(loverCloudUser);

            return CreatedAtRoute("Register",new { loverCloudUser.Id } , loverCloudUserResource);
        }

        [Authorize]
        [HttpGet("info")]
        public async Task<IActionResult> GetLoverCloudUser([FromQuery]string fields)
        {
            string id = GetLoverCloudUserId();
            if (string.IsNullOrEmpty(id)) return Unauthorized();

            var user = await _repository.FindByIdAsync(id);
            var userResource = _mapper.Map<LoverCloudUserResource>(user);
            if(userResource.Spouse != null)
            {
                userResource.Spouse.Spouse = null;
                userResource.Spouse.ReceivedLoverRequests = null;
            }

            if (userResource.LoverRequests.Count > 0)
            {
                foreach (var loverRequest in userResource.LoverRequests)
                {
                    loverRequest.Requester = null;
                    if (userResource.Spouse != null) 
                        loverRequest.Receiver = null;
                }
            }

            var shapedUserResource = userResource.ToDynamicObject(fields);

            return Ok(shapedUserResource);
        }

        /// <summary>
        /// 获取通过身份认证的用户的Id
        /// </summary>
        /// <returns> 经过身份认证的用户的Id </returns>
        private string GetLoverCloudUserId() =>
            User?.Claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;
    }
}
