namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;

    /// <summary>
    /// 标签
    /// </summary>
    public class Tag : IEntity
    {
        public Tag()
        {
            Id = Guid.NewGuid().ToString();
        }

        public const byte NameMaxLength = 30;

        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        public string LoverAlbumId { get; set; }
        public virtual LoverAlbum LoverAlbum { get; set; }

        public string LoverPhotoId { get; set; }
        public virtual LoverPhoto LoverPhoto { get; set; }
    }
}
