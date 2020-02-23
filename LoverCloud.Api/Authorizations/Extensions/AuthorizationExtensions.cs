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
            services.AddScoped<IAuthorizationHandler, MustFemaleHandler>();
            services.AddSingleton<IAuthorizationHandler, LoverCloudUserFieldsHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.LoverResourcePolicy, policy =>
                {
                    policy.AddRequirements(new MustHaveLoverRequirement());
                });
                options.AddPolicy(AuthorizationPolicies.MenstruationLogPolicy, policy =>
                {
                    policy.AddRequirements(new MustFemaleRequirement());
                });
            });
        }
    }
}
