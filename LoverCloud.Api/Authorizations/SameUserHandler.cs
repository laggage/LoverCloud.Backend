namespace LoverCloud.Api.Authorizations
{
    using LoverCloud.Api.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;

    class SameUserRequirement:IAuthorizationRequirement
    {
        public string UserId { get; set; }

        public SameUserRequirement(string userId)
        {
            UserId = userId;
        }
    }

    class SameUserHandler : AuthorizationHandler<SameUserRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            SameUserRequirement requirement)
        {
            if(context.User.GetUserId() == requirement.UserId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
