namespace LoverCloud.Infrastructure.Extensions
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Infrastructure.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class RepositoryExtentions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILoverRepository, LoverRepository>();
        }
    }
}
