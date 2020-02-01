﻿namespace LoverCloud.Core.Models
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
            Guid = System.Guid.NewGuid().ToString();
        }

        public string Guid { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }

        public string LoverAlbumGuid { get; set; }
        public virtual LoverAlbum LoverAlbum { get; set; }

        public string LoverPhotoGuid { get; set; }
        public virtual LoverPhoto LoverPhoto { get; set; }
    }
}
