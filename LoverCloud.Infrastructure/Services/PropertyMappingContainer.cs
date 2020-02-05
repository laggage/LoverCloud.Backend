namespace LoverCloud.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PropertyMappingContainer : IPropertyMappingContainer
    {
        protected readonly IList<IPropertyMapping> _propertyMappings;

        public PropertyMappingContainer()
        {
            _propertyMappings = new List<IPropertyMapping>();
        }

        public virtual void Register<T>() where T : IPropertyMapping, new()
        {
            _propertyMappings.Add(new T());
        }

        public virtual IPropertyMapping Resolve<TSource, TDestination>()
        {
            return _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>()
                ?.FirstOrDefault() ?? 
                   throw new InvalidCastException($"Cannot find property mapping instance for {typeof(TSource).Name}, {typeof(TDestination).Name}");
        }

        public virtual bool ValidMappingExistFor<TSource, TDestination>(string fields)
        {
            var propertyMapping = Resolve<TSource, TDestination>();
            string[] splitedFields = fields.Split(',');
            foreach (string field in splitedFields)
            {
                string trimmedField = field.Trim();
                if(propertyMapping.PropertyDictionary.Keys.Contains(
                    trimmedField, StringComparer.OrdinalIgnoreCase))
                    continue;
                return false;
            }

            return true;
        }
    }
}
