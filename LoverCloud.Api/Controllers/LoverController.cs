namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.JsonPatch;
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
                loverRequestToUpdate.LoverGuid == null)
            {
                var users = new LoverCloudUser[]
                {
                    loverRequestToUpdate.Receiver,
                    loverRequestToUpdate.Requester
                };

                var lover = new Lover
                {
                    Male = users.FirstOrDefault(x => x.Sex == Sex.Male),
                    Female = users.FirstOrDefault(x => x.Sex == Sex.Female),
                    RegisterDate = DateTime.Now
                };
                if (lover.Male == null || lover.Female == null)
                    throw new Exception("操作失败");
                await _loverRepository.AddLoverAsync(lover);
            }
            
            if (!await _unitOfWork.SaveChangesAsync()) throw new Exception("保存数据到数据库失败");
            
            return NoContent();
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
            User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;

    }
}
