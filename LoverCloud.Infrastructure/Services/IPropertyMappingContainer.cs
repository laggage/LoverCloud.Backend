namespace LoverCloud.Infrastructure.Services
{
    public interface IPropertyMappingContainer
    {
        void Register<T>() where T : IPropertyMapping, new();
        IPropertyMapping Resolve<TSource, TDestination>();
        bool ValidMappingExistFor<TSource, TDestination>(string fields);
    }
}
