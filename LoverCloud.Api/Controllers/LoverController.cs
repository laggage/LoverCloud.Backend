namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
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

        [HttpPost("logs")]
        public async Task<IActionResult> CreateLoverLogs([FromBody]LoverLogAddResource addResource)
        {
            var lover = await _loverRepository.GetLoverByLoverCloudUserIdAsync(GetLoverCloudUserId());
            if (lover == null) return NoContent();

            var loverLog = _mapper.Map<LoverLog>(addResource);
            loverLog.Lover = lover;
            if (loverLog == null)
                return NoContent();
            await _loverRepository.AddLoverLogAsync(loverLog);
            var res = await _unitOfWork.SaveChangesAsync();
            if (!res) return NoContent();
            throw new NotImplementedException();
        }

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
            User.Claims.First(x => x.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;

    }
}
