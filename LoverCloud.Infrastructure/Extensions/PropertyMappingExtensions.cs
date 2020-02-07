namespace LoverCloud.Infrastructure.Extensions
{
    using LoverCloud.Infrastructure.Resources;
    using LoverCloud.Infrastructure.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class PropertyMappingExtensions
    {
        public static void AddPropetyMapping(this IServiceCollection services)
        {
            var propetyMappingContainer = new PropertyMappingContainer();
            propetyMappingContainer.Register<LoverPhotoResourceMapping>();
            propetyMappingContainer.Register<LoverAlbumResourceMapping>();
            propetyMappingContainer.Register<LoverLogResourceMapping>();
            services.AddSingleton<IPropertyMappingContainer>(propetyMappingContainer);
        }
    }
}
