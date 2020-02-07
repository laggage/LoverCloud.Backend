namespace LoverCloud.Infrastructure.Resources
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;

    public class LoverCloudUserResource : Resource
    {
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
        public string Password { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string UserName { get; set; }
        public DateTime Birth { get; set; }
        public IFormFile ProfileImage { get; set; }
    }

    public class LoverCloudUserUpdateResource
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime Birth { get; set; }
    }
}
