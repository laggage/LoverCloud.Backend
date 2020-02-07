namespace LoverCloud.Infrastructure.Resources
{
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Services;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class LoverLogResourceMapping : PropertyMapping<LoverLogResource, LoverLog>
    {
        public LoverLogResourceMapping()
            : base(new Dictionary<string, IList<MappedProperty>>
            {
                {
                    nameof(LoverLogResource.CreateDateTime),
                    new MappedProperty[]
                    {
                        new MappedProperty(nameof(LoverLog.CreateDateTime))
                    }
                }
            })
        {
        }
    }
}
