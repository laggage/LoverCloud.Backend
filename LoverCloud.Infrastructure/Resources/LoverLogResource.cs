namespace LoverCloud.Infrastructure.Resources
{
    using System;
    using System.Collections.Generic;

    public class LoverLogResource : Resource
    {
        public string Content { get; set; }
        public DateTime CreateDateTime { get; set; }
        public IList<LoverPhotoResource> LoverPhotos { get; set; }
    }

    public class LoverLogAddResource
    {
        /// <summary>
        /// 最大长度: 1024
        /// </summary>
        public string Content { get; set; }
        public IList<LoverPhotoAddResource> LoverPhotos { get; set; }
    }
}
