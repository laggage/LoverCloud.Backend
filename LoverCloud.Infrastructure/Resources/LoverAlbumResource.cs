namespace LoverCloud.Infrastructure.Resources
{
    using System.Collections.Generic;

    public class LoverAlbumResource
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string CreateDate { get; set; }
        public IList<LoverPhotoResource> Photos { get; set; }
        public IList<TagResource> Tags { get; set; }
    }

    public class LoverAlbumAddResource
    {
        public string Name { get; set; }
        public IList<TagAddResource> Tags { get; set; }
    }
}
