namespace LoverCloud.Infrastructure.Resources
{
    using System;
    using System.Collections.Generic;

    public class MenstruationDescriptionResource : Resource
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class MenstruationLogResource
    {
        public string Guid { get; set; }
        public DateTime StartDate { get; set; }
        public string EndDate { get; set; }
        public IList<MenstruationDescriptionResource> MenstruationDescriptionResources { get; set; }
    }
}
