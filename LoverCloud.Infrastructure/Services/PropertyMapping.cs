namespace LoverCloud.Infrastructure.Services
{
    using System.Collections.Generic;

    public abstract class PropertyMapping<TSource, TDestination> : IPropertyMapping
    {
        public IDictionary<string, IList<MappedProperty>> PropertyDictionary { get; set; }

        public PropertyMapping(IDictionary<string, IList<MappedProperty>> propertyDictionary)
        {
            PropertyDictionary = propertyDictionary;
        }
    }
}
