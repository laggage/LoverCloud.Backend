namespace LoverCloud.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.AspNetCore.Identity;

    public enum Sex
    {
        [Description("男")]
        Male = 1, 
        [Description("女")]
        Female
    }

    public class LoverCloudUser : IdentityUser, IEquatable<LoverCloudUser>
    {
        public LoverCloudUser() : base()
        {
        }

        public LoverCloudUser(string userName) : this()
        {
            UserName = userName;
        }

        public DateTime Birth { get; set; }
        public DateTime RegisterDate { get; set; }
        public Sex Sex { get; set; }
        /// <summary>
        /// 表示用户头像
        /// </summary>
        public string ProfileImage { get; set; }

        public virtual Lover Lover { get; set; }
        public virtual IList<LoverRequest> LoverRequests { get; set; }
        public virtual IList<LoverRequest> ReceivedLoverRequests { get; set; }
        public virtual IList<MenstruationLog> MenstruationLogs { get; set; }

        public LoverCloudUser GetSpouse()
        {
            if (Lover == null) return null;
            return Lover?.LoverCloudUsers.FirstOrDefault(x => !x.Equals(this));
        }

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
