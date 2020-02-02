namespace LoverCloud.Infrastructure.Resources
{
    public class LoverRequestResource
    {
        public string Guid { get; set; }
        public LoverCloudUserResource Requester { get; set; }
        public bool Succeed { get; set; }
    }

    public class LoverRequestAddResource
    {
        public string ReceiverGuid { get; set; }
    }

    public class LoverRequestUpdateResource
    {
        public bool Succeed { get; set; }
    }
}
