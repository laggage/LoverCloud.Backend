namespace LoverCloud.Infrastructure.Resources
{
    using System;

    public class LoverAnniversaryResource : Resource
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class LoverAnniversaryAddResource
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
