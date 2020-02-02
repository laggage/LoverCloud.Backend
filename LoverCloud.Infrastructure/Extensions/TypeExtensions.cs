namespace LoverCloud.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetProperties(this Type type, string fields)
        {
            if (string.IsNullOrEmpty(fields))
                return type.GetProperties(BindingFlags.Public|BindingFlags.Instance);
            var propertyInfoList = new List<PropertyInfo>();
            var splitFields = fields.Split(',');
            foreach (string splitField in splitFields)
            {
                var property = type.GetProperty(splitField);
                if (property == null) continue;
                propertyInfoList.Add(property);
            }

            return propertyInfoList;
        }
    }
}
