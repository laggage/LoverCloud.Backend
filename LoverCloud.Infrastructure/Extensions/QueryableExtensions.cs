namespace LoverCloud.Infrastructure.Extensions
{
    using LoverCloud.Infrastructure.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Text;

    public static class QueryableExtensions
    {
        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy">指定排序字段, 例如 name desc, age, ...</param>
        /// <param name="propertyMapping"></param>
        public static IEnumerable<T> ApplySort<T>(
            this IQueryable<T> source, in string orderBy, in IPropertyMapping propertyMapping)
        {
            if (source == null) throw new ArgumentNullException();
            if (propertyMapping == null) throw new ArgumentNullException();

            var fields = orderBy.Split(",").Reverse();
            foreach (string field in fields)
            {
                string trimmedField = field.Trim();
                string[] trimmedFields = trimmedField.Split(' ');
                bool desc = trimmedFields.Length == 2 && trimmedFields[1] == "desc";
                string property = trimmedFields[0];
                property = propertyMapping.PropertyDictionary.Keys.FirstOrDefault(
                    x => x.Equals(property, StringComparison.OrdinalIgnoreCase));
                if (!propertyMapping.PropertyDictionary.TryGetValue(property, out IList<MappedProperty> mappedProperties))
                    throw new InvalidCastException($"key mapping for {property} is missing");

                foreach (var mappedProperty in mappedProperties)
                {
                    desc = mappedProperty.Revert ? !desc : desc;
                    source = source.OrderBy(
                        $"{mappedProperty.Name} {(desc ? "descending" : "ascending")}");
                }
            }
            return source;
        }
    }
}
