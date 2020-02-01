namespace LoverCloud.Api.Extensions
{
    using AutoMapper;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Resources;

    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Tag, TagResource>();
            CreateMap<TagAddResource, Tag>();

            CreateMap<LoverAlbum, LoverAlbumResource>();
            CreateMap<LoverAlbumAddResource, LoverAlbum>();

            CreateMap<LoverPhoto, LoverPhotoResource>();
            CreateMap<LoverPhotoAddResource, LoverPhoto>();

            CreateMap<LoverLog, LoverLogResource>();
            CreateMap<LoverLogAddResource, LoverLog>();

            CreateMap<LoverAnniversary, LoverAnniversaryResource>();

            CreateMap<MenstruationDescription, MenstruationDescriptionResource>();
            CreateMap<MenstruationLog, MenstruationLogResource>();

            CreateMap<LoverCloudUser, LoverCloudUserResource>()
                .ForMember(x => x.Guid, c => c.MapFrom(s => s.Id));
            CreateMap<LoverCloudUserAddResource, LoverCloudUser>();
        }
    }
}
