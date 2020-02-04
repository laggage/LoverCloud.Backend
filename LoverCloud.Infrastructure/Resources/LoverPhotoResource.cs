namespace LoverCloud.Infrastructure.Resources
{
    using System;
    using System.Collections.Generic;

    public class LoverPhotoResource
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime PhotoTakenDateTime { get; set; }
        public LoverAlbumResource Album { get; set; }
        public IList<TagResource> Tags { get; set; }
    }

    public class LoverPhotoAddResource
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<TagAddResource> Tags { get; set; }
    }
}