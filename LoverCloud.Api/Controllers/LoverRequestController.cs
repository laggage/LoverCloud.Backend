namespace LoverCloud.Api.Controllers
{
    using LoverCloud.Api.Extensions;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.JsonPatch;
    using AutoMapper;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Authorize]
    [Route("api/lovers/loverrequests")]
    public class LoverRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoverRequestRepository _repository;
        private readonly ILoverRepository _loverRepository;
        private readonly UserManager<LoverCloudUser> _userManager;

        public LoverRequestController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoverRequestRepository repository,
            ILoverRepository loverRepository,
            UserManager<LoverCloudUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
            _loverRepository = loverRepository;
            _userManager = userManager;
        }

        [HttpPost(Name = "AddLoverRequest")]
        public async Task<IActionResult> Post(LoverRequestAddResource addResource)
        {
            var requester = await _userManager.FindByIdAsync(this.GetUserId());
            if (requester == null) return Unauthorized();
            var receiver = await _userManager.FindByIdAsync(addResource.ReceiverId);
            if (receiver == null) return BadRequest($"找不到Guid为 {addResource.ReceiverId} 的用户");

            if (receiver.Lover != null)
                return BadRequest("对方已有情侣, 无法发出请求.");

            var loverRequest = _mapper.Map<LoverRequest>(addResource);
            loverRequest.Receiver = receiver;
            loverRequest.Requester = requester;
            loverRequest.Succeed = null;
            loverRequest.RequestDate = DateTime.Now;

            _repository.Add(loverRequest);
            var result = await _unitOfWork.SaveChangesAsync();
            if (!result) throw new Exception();
            var loverRequestResource = _mapper.Map<LoverRequestResource>(loverRequest);

            return CreatedAtRoute("AddLoverRequest", new { loverRequest.Id }, loverRequestResource);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchLoverRequest([FromRoute]string id, [FromBody] JsonPatchDocument<LoverRequestUpdateResource> loverRequestPatchDocument)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var loverRequestToUpdate = await _repository.FindByIdAsync(id);
            if (loverRequestToUpdate == null) return BadRequest($"找不到对应的 LoverRequest({id})");
            string userId = this.GetUserId();
            if (!(loverRequestToUpdate.ReceiverId == userId))
                return BadRequest("非法用户, 无权操作");

            var loverRequestToUpdateResource = _mapper.Map<LoverRequestUpdateResource>(loverRequestToUpdate);
            loverRequestPatchDocument.ApplyTo(loverRequestToUpdateResource);
            _mapper.Map(loverRequestToUpdateResource, loverRequestToUpdate);

            if (loverRequestToUpdateResource.Succeed == true &&
                loverRequestToUpdate.LoverId == null &&
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
                _loverRepository.Add(lover);
            }

            if (!await _unitOfWork.SaveChangesAsync()) throw new Exception("保存数据到数据库失败");

            return NoContent();
        }

    }
}
