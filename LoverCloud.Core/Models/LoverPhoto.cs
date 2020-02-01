namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;
    using System.Collections.Generic;

    public class LoverPhoto : IEntity
    {
        public LoverPhoto()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; set; }
        /// <summary>
        /// 照片上传日期
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 照片拍摄时间
        /// </summary>
        public DateTime PhotoTakenDate { get; set; }
        /// <summary>
        /// 照片描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 照片标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 照片资源路径
        /// </summary>
        public string Url { get; set; }

        public string AlbumGuid { get; set; }
        public virtual LoverAlbum Album { get; set; }

        public virtual Lover Lover { get; set; }

        public virtual LoverLog LoverLog { get; set; }

        public virtual IList<Tag> Tags { get; set; }
    }
}
