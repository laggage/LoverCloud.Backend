namespace LoverCloud.Core.Models
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    public enum Sex { Male = 1, Female }

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

        public virtual Lover Lover { get; set; }

        public virtual LoverRequest LoverRequest { get; set; }
        public virtual IList<LoverRequest> ReceivedLoverRequests { get; set; }
        public virtual IList<MenstruationLog> MenstruationLogs { get; set; }
    }
}
