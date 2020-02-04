namespace LoverCloud.Infrastructure.Services
{
    using System.Collections.Generic;

    public interface IPropertyMapping
    {
        IDictionary<string, IList<MappedProperty>> PropertyDictionary { get; set; }
    }
}
