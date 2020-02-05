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
            Type type = source.GetType();
            if (string.IsNullOrEmpty(fields))
            {
                propertyInfoList.AddRange(
                    type.GetProperties(
                        BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public));
            }
            else
            {
                string[] splitFields = fields.Split(',');
                foreach (var field in splitFields)
                {
                    string trimmedField = field.Trim();
                    PropertyInfo property = type.GetProperty(
                        trimmedField, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                    if (property == null) continue;
                    propertyInfoList.Add(property);
                }
            }
            
            var dynamicObj = new ExpandoObject();
            foreach (var propertyInfo in propertyInfoList)
                dynamicObj.TryAdd(propertyInfo.Name, propertyInfo.GetValue(source));
            
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
