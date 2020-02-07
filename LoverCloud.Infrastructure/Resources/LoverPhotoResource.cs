namespace LoverCloud.Infrastructure.Resources
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;

    public class LoverPhotoResource
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime PhotoTakenDate { get; set; }
        public LoverAlbumResource Album { get; set; }
        public IList<TagResource> Tags { get; set; }
        public object ToDynamicObjec { get; set; }
    }

    public class LoverPhotoAddResource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TagAddResource> Tags { get; set; }
        /// <summary>
        /// 照片拍摄时间
        /// </summary>
        public DateTime PhotoTakenDate { get; set; }
        /// <summary>
        /// 照片文件
        /// </summary>
        public IFormFile File { get; set; }
    }

    public class LoverPhotoUpdateResource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TagAddResource> Tags { get; set; }
        /// <summary>
        /// 照片拍摄时间
        /// </summary>
        public DateTime PhotoTakenDate { get; set; }
    }
}