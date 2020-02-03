namespace LoverCloud.Infrastructure.Resources
{
    using System;
    using System.Collections.Generic;

    public class LoverCloudUserResource
    {
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Sex { get; set; }
        public MenstruationLogResource MenstruationLog { get; set; }    
        public LoverCloudUserResource Spouse { get; set; }
        public IList<LoverRequestResource> ReceivedLoverRequests { get; set; }
        public IList<LoverRequestResource> LoverRequests { get; set; }
    }

    public class LoverCloudUserAddResource
    {
        public string Email { get; set; }
        public string Sex { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime Birth { get; set; }
    }
}
