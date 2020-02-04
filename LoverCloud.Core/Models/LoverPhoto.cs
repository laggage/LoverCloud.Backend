namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;

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
        /// 照片名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 照片资源路径
        /// </summary>
        public string PhotoUrl { get; set; }
        /// <summary>
        /// 图片文件在磁盘上的物理路径
        /// </summary>
        public string PhotoPhysicalPath { get; set; }

        public string AlbumGuid { get; set; }
        public virtual LoverAlbum Album { get; set; }

        public virtual Lover Lover { get; set; }

        public virtual LoverLog LoverLog { get; set; }

        public virtual IList<Tag> Tags { get; set; }

        public virtual LoverCloudUser Uploader { get; set; }

        /// <summary>
        /// 生成照片文件路径
        /// </summary>
        /// <param name="fileSuffix">照片文件后缀名(比如: jpg, png, ...)</param>
        /// <returns>照片文件完整的物理路径</returns>
        public string GeneratePhotoPhysicalPath(string fileSuffix)
        {
            if (Uploader == null)
                throw new InvalidOperationException("The property \"Uploader\" of the instance is null, cannot get the directory for the photo to save.");
            return Path.Combine(
                Directory.GetCurrentDirectory(),
                "LoverCloudResources", "UserResources",
                $"{Uploader.UserName}-{Uploader.Id}", $"{Guid}.{fileSuffix}");
        }

        /// <summary>
        /// 根据 <see cref="PhotoPhysicalPath"/> 删除物理磁盘上的图片文件
        /// </summary>
        public void DeletePhyicalFile()
        {
            if (File.Exists(PhotoPhysicalPath))
                File.Delete(PhotoPhysicalPath);
        }
    }
}
