namespace LoverCloud.Api.Authorizations.Extensions
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;

    public static class AuthorizationExtensions
    {
        public static void AddAppAuthorization(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, MustHaveLoverHandler>();
            services.AddScoped<IAuthorizationHandler, LoverResourceCrudHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.LoverResourcePolicy, policy =>
                {
                    policy.AddRequirements(new MustHaveLoverRequirement());
                });
            });
        }
    }
}
