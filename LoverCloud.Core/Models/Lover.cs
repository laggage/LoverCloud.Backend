namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;
    using System.Collections.Generic;

    public class Lover : IEntity
    {
        public Lover()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; set; }
        /// <summary>
        /// 男方
        /// </summary>
        public virtual LoverCloudUser Male { get; set; }
        public string MaleGuid { get; set; }
        /// <summary>
        /// 女方
        /// </summary>
        public virtual LoverCloudUser Female { get; set; }
        public string FemaleGuid { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterDate { get; set; }
        /// <summary>
        /// 男方是否为初恋
        /// </summary>
        public bool IsBoyFirstLove { get; set; }
        /// <summary>
        /// 女方是否为初恋
        /// </summary>
        public bool IsGirlFirstLove { get; set; }

        public virtual IList<LoverAnniversary> LoverAnniversaries { get; set; }
        public virtual LoverRequest LoverRequest { get; set; }
        public virtual IList<LoverAlbum> LoverAlbums { get; set; }
        public virtual IList<LoverPhoto> LoverPhotos { get; set; }
        public virtual IList<LoverLog> LoverLogs { get; set; }
        
    }

    /// <summary>
    /// 情侣纪念日表
    /// </summary>
    public class LoverAnniversary : IEntity
    {
        public LoverAnniversary()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; set; }
        public virtual Lover Lover { get; set; }
        /// <summary>
        /// 纪念日名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 纪念日的具体日期
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 纪念日的描述
        /// </summary>
        public string Description { get; set; }
    }
}
