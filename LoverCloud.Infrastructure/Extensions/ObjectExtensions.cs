namespace LoverCloud.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Dynamic;

    public static class ObjectExtensions
    {
        public static ExpandoObject ToDynamicObject(this object source, string fields = null)
        {
            var propertyInfoList = source.GetType().GetProperties(fields);
            var dynamicObj = new ExpandoObject();

            foreach (var propertyInfo in propertyInfoList)
            {
                dynamicObj.TryAdd(propertyInfo.Name, propertyInfo.GetValue(source));
            }

            return dynamicObj;
        }
    }
}
