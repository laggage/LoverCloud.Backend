namespace LoverCloud.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Dynamic;

    public static class DynamicExtensions
    {
        /// <summary>
        /// 获得ExpandoObject某个键值对的值, 将其作为字符串返回
        /// </summary>
        /// <param name="obj"> <see cref="ExpandoObject"/> </param>
        /// <param name="key">键</param>
        /// <returns> 如果能够根据键值获得对应的值, 则将该值作为字符串返回, 否则返回null </returns>
        public static string GetOrDefault(this ExpandoObject obj, string key)
        {
            var dict = obj as IDictionary<string, object>;
            if (dict.TryGetValue(key, out object value))
                return value as string;
            else return null;
        }
    }
}
