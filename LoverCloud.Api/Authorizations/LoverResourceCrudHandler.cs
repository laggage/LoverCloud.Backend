namespace LoverCloud.Api.Authorizations
{
    using LoverCloud.Api.Extensions;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using System.Threading.Tasks;

    public class LoverResourceCrudHandler : AuthorizationHandler<OperationAuthorizationRequirement, ILoverResource>
    {
        private readonly ILoverRepository _loverRepository;

        public LoverResourceCrudHandler(ILoverRepository loverRepository)
        {
            _loverRepository=loverRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, 
            ILoverResource resource)
        {
            if(requirement.Name == Operations.Update.Name ||
               requirement.Name == Operations.Delete.Name)
            {
                if (string.IsNullOrEmpty(resource.LoverId))
                    return;
                Lover lover = await _loverRepository.FindByUserIdAsync(context.User.GetUserId());
                if (lover?.Id == resource.LoverId)
                    context.Succeed(requirement);
            }
        }
    }
}
