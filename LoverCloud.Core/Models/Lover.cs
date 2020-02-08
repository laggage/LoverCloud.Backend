namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 情侣实体
    /// </summary>
    public class Lover : IEntity
    {
        public Lover()
        {
            Id = Guid.NewGuid().ToString();
            LoverAnniversaries = new List<LoverAnniversary>();
            LoverAlbums = new List<LoverAlbum>();
            LoverPhotos = new List<LoverPhoto>();
            LoverLogs = new List<LoverLog>();
            LoverCloudUsers = new List<LoverCloudUser>();
        }

        public string Id { get; set; }

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
        /// <summary>
        /// 情侣纪念日导航属性
        /// </summary>
        public virtual IList<LoverAnniversary> LoverAnniversaries { get; set; }
        /// <summary>
        /// 情侣请求导航属性
        /// </summary>
        public virtual LoverRequest LoverRequest { get; set; }
        /// <summary>
        /// 情侣相册导航属性
        /// </summary>
        public virtual IList<LoverAlbum> LoverAlbums { get; set; }
        /// <summary>
        /// 情侣照片导航属性
        /// </summary>
        public virtual IList<LoverPhoto> LoverPhotos { get; set; }
        /// <summary>
        /// 情侣日志导航属性
        /// </summary>
        public virtual IList<LoverLog> LoverLogs { get; set; }
        /// <summary>
        /// 情侣成员导航属性
        /// Lover - LoverCloudUser 为 一对多 的关系 (一个 Lover 应该包含两个LoverCloudUser)
        /// </summary>
        public virtual IList<LoverCloudUser> LoverCloudUsers { get; set; }

        public bool HasUser(string userId) => LoverCloudUsers.Any(x => x.Id == userId);

        public bool HasUser(LoverCloudUser user) => HasUser(user.Id);

        public LoverCloudUser GetUser(string userId) => LoverCloudUsers.FirstOrDefault(x => x.Id == userId);
    }

    /// <summary>
    /// 情侣纪念日表
    /// </summary>
    public class LoverAnniversary : IEntity
    {
        public LoverAnniversary()
        {
            Id = Guid.NewGuid().ToString();
        }

        public const byte NameMaxLength = 30;
        public const int DescriptionMaxLength = 512;

        public string Id { get; set; }
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
