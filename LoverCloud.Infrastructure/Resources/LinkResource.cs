namespace LoverCloud.Infrastructure.Resources
{
    public class LinkResource
    {
        public LinkResource(string rel, string method, string url)
        {
            Rel = rel;
            Method = method;
            Url = url;
        }

        public string Rel { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
    }
}
