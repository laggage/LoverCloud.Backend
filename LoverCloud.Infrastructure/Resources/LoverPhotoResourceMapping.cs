namespace LoverCloud.Infrastructure.Resources
{
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Services;
    using System.Collections.Generic;

    public class LoverPhotoResourceMapping : PropertyMapping<LoverPhotoResource, LoverPhoto>
    {
        public LoverPhotoResourceMapping() :
            base(new Dictionary<string, IList<MappedProperty>>
            {
                {
                    nameof(LoverPhotoResource.Name), new List<MappedProperty>
                    {
                        new MappedProperty(nameof(LoverPhoto.Name))
                    }
                },
                {
                    nameof(LoverPhotoResource.PhotoTakenDate), new List<MappedProperty>
                    {
                        new MappedProperty(nameof(LoverPhoto.PhotoTakenDate))
                    }
                },
            })
        {
        }
    }
}