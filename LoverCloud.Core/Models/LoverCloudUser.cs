namespace LoverCloud.Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Microsoft.AspNetCore.Identity;

    public enum Sex
    {
        [Description("男")]
        Male = 1, 
        [Description("女")]
        Female
    }

    public class LoverCloudUser : IdentityUser
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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            if (ReferenceEquals(obj, this)) return true;
            return string.Equals(Id, (obj as LoverCloudUser).Id);
        }

        public override int GetHashCode()
        {
            return Id?.GetHashCode() ?? base.GetHashCode();
        }
    }
}
