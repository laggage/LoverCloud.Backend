namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 姨妈记录
    /// </summary>
    public class MenstruationLog : IEntity
    {
        public MenstruationLog()
        {
            Id = System.Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        /// <summary>
        /// 姨妈开始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 姨妈结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 每一天对姨妈的感觉的描述
        /// </summary>
        public virtual IList<MenstruationDescription> MenstruationDescriptions { get; set; }

        public virtual LoverCloudUser LoverCloudUser { get; set; }
    }
}
