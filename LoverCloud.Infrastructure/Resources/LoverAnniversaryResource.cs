namespace LoverCloud.Infrastructure.Resources
{
    using System;

    public class LoverAnniversaryResource
    {
        public string Guid { get; set; }
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
