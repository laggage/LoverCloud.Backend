namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 情侣相册
    /// </summary>
    public class LoverAlbum : IEntity
    {
        public LoverAlbum()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; set; }
        /// <summary>
        /// 相册名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 相册创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 相册上次更新时间
        /// </summary>
        public DateTime LastUpdate { get; set; }
        /// <summary>
        /// 相册描述
        /// </summary>
        public string Description { get; set; }

        public virtual IList<LoverPhoto> Photos { get; set; }

        public string LoverGuid { get; set; }
        public virtual Lover Lover { get; set; }

        /// <summary>
        /// 相册标签
        /// </summary>
        public virtual IList<Tag> Tags { get; set; }
    }
}
