namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;

    /// <summary>
    /// 姨妈期间每一天对姨妈的描述
    /// </summary>
    public class MenstruationDescription : IEntity
    {
        public MenstruationDescription()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
        public string Guid { get; set; }
        /// <summary>
        /// 姨妈期间的某一天
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 这一天的感觉的描述
        /// </summary>
        public string Description { get; set; }

        public virtual MenstruationLog MenstruationLog { get; set; }
    }
}
