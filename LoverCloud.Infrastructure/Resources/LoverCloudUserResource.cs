namespace LoverCloud.Infrastructure.Resources
{
    using LoverCloud.Core.Models;
    using System;
    using System.Collections.Generic;

    public class LoverCloudUserResource
    {
        public string Guid { get; set; }
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }
        public Sex Sex { get; set; }
        public MenstruationLogResource MenstruationLogResource { get; set; }    
    }

    public class LoverCloudUserAddResource
    {
        public string Email { get; set; }
        public Sex Sex { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime Birth { get; set; }
    }
}
