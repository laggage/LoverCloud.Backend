namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/users")]
    public class LoverCloudUserController : ControllerBase
    {
        private readonly UserManager<LoverCloudUser> _userManager;
        private readonly IMapper _mapper;

        public LoverCloudUserController(UserManager<LoverCloudUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
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

            var res = await _userManager.CreateAsync(loverCloudUser, addResource.Password);
            if (!res.Succeeded) return NoContent();

            var loverCloudUserResource = _mapper.Map<LoverCloudUserResource>(loverCloudUser);

            return CreatedAtRoute("Register",new { loverCloudUser.Id } , loverCloudUserResource);

            throw new NotImplementedException();
        }
    }
}
