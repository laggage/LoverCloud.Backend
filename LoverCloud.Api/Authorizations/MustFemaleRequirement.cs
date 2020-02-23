namespace LoverCloud.Api.Authorizations
{
    using LoverCloud.Api.Extensions;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;

    public class MustFemaleHandler : AuthorizationHandler<MustFemaleRequirement>
    {
        private readonly ILoverCloudUserRepository _userRepository;

        public MustFemaleHandler(ILoverCloudUserRepository userRepository)
        {
            _userRepository=userRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MustFemaleRequirement requirement)
        {
            LoverCloudUser user = await _userRepository.FindByIdAsync(context.User.GetUserId());
            if (user != null && user.Sex == Sex.Female)
                context.Succeed(requirement);
        }
    }

    public class MustFemaleRequirement : IAuthorizationRequirement
    {
    }
}
