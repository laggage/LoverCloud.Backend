namespace LoverCloud.Api.Authorizations
{
    using LoverCloud.Api.Extensions;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class MustHaveLoverRequirement : IAuthorizationRequirement
    {
    }

    /// <summary>
    /// 用户必须拥有一个情侣才能获得访问权限
    /// </summary>
    public class MustHaveLoverHandler : AuthorizationHandler<MustHaveLoverRequirement>
    {
        // TODO: 不要直接和DbContext交互
        private readonly LoverCloudDbContext _dbContext;

        public MustHaveLoverHandler(LoverCloudDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            MustHaveLoverRequirement requirement)
        {
            if (await HasLover(context.User.GetUserId()))
                context.Succeed(requirement);
        }

        private async Task<bool> HasLover(string userId)
        {
            return !string.IsNullOrEmpty(
                (await _dbContext.Users.FirstOrDefaultAsync(
                    x => x.Id == userId))?.LoverId);
        }
    }
}
