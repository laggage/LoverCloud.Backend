namespace LoverCloud.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Reflection;

    public static class ObjectExtensions
    {
        public static ExpandoObject ToDynamicObject(this object source, string fields = null)
        {
            var propertyInfoList = new List<PropertyInfo>();
            string[] splitFields = fields.Split(',');

            Type type = source.GetType();
            var dynamicObj = new ExpandoObject();
            PropertyInfo property = null;
            foreach (var field in splitFields)
            {
                string trimmedField = field.Trim();
                property = type.GetProperty(
                    trimmedField, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (property == null) continue;
                dynamicObj.TryAdd(property.Name, property.GetValue(source));
            }
            
            return dynamicObj;
        }

        public static IEnumerable<ExpandoObject> ToDynamicObject<T>(this IEnumerable<T> source, string fields)
        {
            var objects = new List<ExpandoObject>();
            foreach (var s in source)
                objects.Add(s.ToDynamicObject(fields));
            return objects;
        }
    }
}
