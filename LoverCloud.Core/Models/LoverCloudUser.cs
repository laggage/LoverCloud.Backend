namespace LoverCloud.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using LoverCloud.Core.Extensions;
    using LoverCloud.Core.Interfaces;
    using Microsoft.AspNetCore.Identity;

    public enum Sex
    {
        [Description("男")]
        Male = 1, 
        [Description("女")]
        Female
    }

    public class LoverCloudUser : IdentityUser, IEquatable<LoverCloudUser>, IEntity
    {
        public LoverCloudUser() : base()
        {
            LoverRequests = new List<LoverRequest>();
            ReceivedLoverRequests = new List<LoverRequest>();
            MenstruationLogs = new List<MenstruationLog>();
            LoverAlbums = new List<LoverAlbum>();
            LoverPhotos = new List<LoverPhoto>();
            LoverLogs = new List<LoverLog>();
            RegisterDate = DateTime.Now;
        }

        public LoverCloudUser(string userName) : this()
        {
            UserName = userName;
        }

        public const byte EmailMaxLength = byte.MaxValue;
        public const byte UserNameMaxLength = 50;

        public DateTime Birth { get; set; }
        public DateTime RegisterDate { get; set; }
        public Sex Sex { get; set; }
        /// <summary>
        /// 表示用户头像所在物理路径
        /// </summary>
        public string ProfileImagePhysicalPath { get; set; }
        public virtual Lover Lover { get; set; }
        /// <summary>
        /// 发出的情侣请求
        /// </summary>
        public virtual IList<LoverRequest> LoverRequests { get; set; }
        /// <summary>
        /// 接收到的情侣请求
        /// </summary>
        public virtual IList<LoverRequest> ReceivedLoverRequests { get; set; }
        /// <summary>
        /// 生理期记录, 只对女用户开放
        /// </summary>
        public virtual IList<MenstruationLog> MenstruationLogs { get; set; }
        /// <summary>
        /// 用户创建的相册
        /// </summary>
        public virtual IList<LoverAlbum> LoverAlbums { get; set; }
        /// <summary>
        /// 用户上传的图片
        /// </summary>
        public virtual IList<LoverPhoto> LoverPhotos { get; set; }
        /// <summary>
        /// 用户发的情侣日志
        /// </summary>
        public virtual IList<LoverLog> LoverLogs { get; set; }

        public string UserPhysicalDirectory => Path.Combine("UserResources", $"{UserName}-{Id}");

        public LoverCloudUser GetSpouse()
        {
            if (Lover == null) return null;
            return Lover?.LoverCloudUsers.FirstOrDefault(x => !x.Equals(this));
        }
        
        /// <summary>
        /// 生成用户头像的保存路径
        /// </summary>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public string GenerateProfileImagePhysicalPath(string suffix) =>
            Path.Combine(
                Directory.GetCurrentDirectory(),
                LoverCloudApiConstraint.ResourcesDirectoryName, UserPhysicalDirectory, "ProfileImage",
                $"{UserName}.{suffix}");

        public override string ToString()
        {
            return $"{UserName}, {DateTime.Now.Year - Birth.Year}岁, {(Sex == Sex.Male ? "男" : "女")}";
        }

        public bool Equals([AllowNull] LoverCloudUser other)
        {
            if (other == null) return false;
            return ReferenceEquals(other, this) || string.Equals(other.Id, Id);
        }
    }
}
