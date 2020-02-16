namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Extensions;
    using LoverCloud.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class LoverPhoto : IEntity
    {
        public LoverPhoto()
        {
            Id = Guid.NewGuid().ToString();
        }

        public const byte NameMaxLength = 30;
        public const int DescriptionMaxLength = 512;
        public const int PhotoUrlMaxLength = 512;
        public const int PhotoPhysicalPathMaxLength = 5120;

        public string Id { get; set; }
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
        public string PhysicalPath { get; set; }


        public string AlbumId { get; set; }
        public virtual LoverAlbum Album { get; set; }

        public virtual Lover Lover { get; set; }

        public virtual LoverLog LoverLog { get; set; }

        public virtual IList<Tag> Tags { get; set; }

        public string UploaderId { get; set; }
        /// <summary>
        /// 上传照片的用户
        /// </summary>
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
                LoverCloudApiConstraint.ResourcesDirectoryName, 
                Uploader.UserPhysicalDirectory, $"{Id}.{fileSuffix}");
        }

        /// <summary>
        /// 根据 <see cref="PhysicalPath"/> 删除物理磁盘上的图片文件
        /// </summary>
        public void DeletePhyicalFile()
        {
            if (File.Exists(PhysicalPath))
                File.Delete(PhysicalPath);
        }
    }
}
