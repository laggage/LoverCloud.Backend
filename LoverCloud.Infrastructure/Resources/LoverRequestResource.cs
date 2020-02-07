namespace LoverCloud.Infrastructure.Resources
{
    public class LoverRequestResource : Resource
    {
        public LoverCloudUserResource Requester { get; set; }
        public LoverCloudUserResource Receiver { get; set; }
        public bool? Succeed { get; set; }
    }

    public class LoverRequestAddResource
    {
        public string ReceiverId { get; set; }
    }

    public class LoverRequestUpdateResource
    {
        public bool? Succeed { get; set; }
    }
}
