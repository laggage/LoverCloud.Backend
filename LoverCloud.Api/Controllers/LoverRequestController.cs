namespace LoverCloud.Api.Controllers
{
    using LoverCloud.Api.Extensions;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.JsonPatch;
    using AutoMapper;
    using LoverCloud.Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using LoverCloud.Api.Authorizations;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using LoverCloud.Infrastructure.Extensions;

    [ApiController]
    [Authorize]
    [Route("api/lovers/loverrequests")]
    public class LoverRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoverRequestRepository _repository;
        private readonly ILoverRepository _loverRepository;
        private readonly ILoverCloudUserRepository _userRepository;
        private readonly UserManager<LoverCloudUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public LoverRequestController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoverRequestRepository repository,
            ILoverRepository loverRepository,
            ILoverCloudUserRepository  userRepository,
            UserManager<LoverCloudUser> userManager,
            IAuthorizationService authorizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
            _loverRepository = loverRepository;
            _userRepository=userRepository;
            _userManager = userManager;
            _authorizationService=authorizationService;
        }

        [HttpPost(Name = "AddLoverRequest")]
        public async Task<IActionResult> Post(LoverRequestAddResource addResource)
        {
            var requester = await _userRepository.FindByIdAsync(this.GetUserId());
            if (requester == null) return Unauthorized();
            // 被请求方已经接收到过该用户的情侣请求
            if (requester.LoverRequests.Any(x => x.ReceiverId.Equals(addResource.ReceiverId)))
                return BadRequest();

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

            return CreatedAtRoute("AddLoverRequest", loverRequestResource);
        }

        [HttpGet(Name = "GetLoverRequests")]
        public async Task<IActionResult> Get(
            [FromQuery]LoverRequestQueryParameters parameters)
        {
            parameters.UserId = string.IsNullOrEmpty(parameters.UserId) ? this.GetUserId() : parameters.UserId;
            string userId = parameters.UserId;
            userId = string.IsNullOrEmpty(userId) ? this.GetUserId() : userId;
            // 鉴权
            AuthorizationResult authorizationResult = await _authorizationService
                .AuthorizeAsync(User, null, new SameUserRequirement(userId));
            if (!authorizationResult.Succeeded) return Forbid();

            PaginatedList<LoverRequest> loverRequests = await _repository.GetAsync(
                parameters);

            IEnumerable<LoverRequestResource> loverRequestResources = _mapper
                .Map<IEnumerable<LoverRequestResource>>(loverRequests)
                .Select(x =>
                {
                    x.Requester.GetProfileImageUrl(Url);
                    x.Receiver.GetProfileImageUrl(Url);
                    return x;
                });

            IEnumerable<ExpandoObject> shapedLoverRequest = loverRequestResources
                .ToDynamicObject(parameters.Fields).Select(x =>
                {
                    var dict = x as IDictionary<string, object>;
                    if(dict.ContainsKey("Receiver") && dict["Receiver"] is LoverCloudUserResource u)
                        dict["Receiver"] = u.ToDynamicObject("id, username, profileImageUrl");
                    if(dict.ContainsKey("Requester") && dict["Requester"] is LoverCloudUserResource uu)
                        dict["Requester"] =  uu.ToDynamicObject("id, username, profileImageUrl");
                    return x;
                });

            this.AddPaginationHeaderToResponse(loverRequests);

            var result = new
            {
                links = this.CreatePaginationLinks(
                    "GetLoverRequests", parameters, 
                    loverRequests.HasPrevious, loverRequests.HasNext),
                value = shapedLoverRequest
            };

            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchLoverRequest([FromRoute]string id, [FromBody] JsonPatchDocument<LoverRequestUpdateResource> loverRequestPatchDocument)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var loverRequestToUpdate = await _repository.FindByIdAsync(id);
            if (loverRequestToUpdate == null) return BadRequest($"找不到对应的 LoverRequest({id})");
            
            AuthorizationResult authorizationResult = await _authorizationService
                .AuthorizeAsync(
                User,
                null,
                new SameUserRequirement(loverRequestToUpdate.ReceiverId));
            if (!(authorizationResult.Succeeded))
                return Forbid();

            var loverRequestToUpdateResource = _mapper.Map<LoverRequestUpdateResource>(
                loverRequestToUpdate);
            loverRequestPatchDocument.ApplyTo(loverRequestToUpdateResource);
            _mapper.Map(loverRequestToUpdateResource, loverRequestToUpdate);

            //if (loverRequestToUpdate.LoverId != null) return BadRequest();

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
            else
            {
                return BadRequest();
            }

            if (!await _unitOfWork.SaveChangesAsync()) throw new Exception("保存数据到数据库失败");

            return NoContent();
        }
    }
}
