namespace LoverCloud.Infrastructure.Resources
{
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Services;
    using System.Collections.Generic;

    public class LoverAlbumResourceMapping : PropertyMapping<LoverAlbumResource, LoverAlbum>
    {
        public LoverAlbumResourceMapping()
            : base(new Dictionary<string, IList<MappedProperty>>
            {
                {
                    nameof(LoverAlbumResource.Name), new MappedProperty[]
                    {
                        new MappedProperty(nameof(LoverAlbum.Name))
                    }
                },
                {
                    nameof(LoverAlbumResource.CreateDate), new MappedProperty[]
                    {
                        new MappedProperty(nameof(LoverAlbum.CreateDate))
                    }
                },
            })
        {
        }
    }
}
