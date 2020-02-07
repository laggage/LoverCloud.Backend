namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;
    using System.Collections.Generic;

    public class LoverLog : IEntity
    {
        public LoverLog()
        {
            Id = Guid.NewGuid().ToString();
        }

        public const int ContentMaxLength = 4096;

        public string Id { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 情侣日志发表时间
        /// </summary>
        public DateTime CreateDateTime { get; set; }
        /// <summary>
        /// 上次修改/更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        public virtual IList<LoverPhoto> LoverPhotos { get; set; }

        public string LoverId { get; set; }
        public virtual Lover Lover { get; set; }
        public string CreaterId { get; set; }
        /// <summary>
        /// 情侣日志发表用户
        /// </summary>
        public virtual LoverCloudUser Creater { get; set; }
    }
}
