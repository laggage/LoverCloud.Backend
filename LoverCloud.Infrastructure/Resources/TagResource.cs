namespace LoverCloud.Infrastructure.Resources
{
    using System;

    public class TagResource
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class TagAddResource
    {
        public string Name { get; set; }
    }
}
