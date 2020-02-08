namespace LoverCloud.Api.Extensions
{
    using AutoMapper;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Extensions;
    using LoverCloud.Infrastructure.Resources;

    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Tag
            CreateMap<Tag, TagResource>();
            CreateMap<TagAddResource, Tag>()
                .ReverseMap();

            // LoverAlbum
            CreateMap<LoverAlbum, LoverAlbumResource>();
            CreateMap<LoverAlbumAddResource, LoverAlbum>();
            CreateMap<LoverAlbumUpdateResource, LoverAlbum>()
                .ReverseMap();

            // LoverPhoto
            CreateMap<LoverPhoto, LoverPhotoResource>();
            CreateMap<LoverPhotoAddResource, LoverPhoto>();
            CreateMap<LoverPhotoUpdateResource, LoverPhoto>()
                .ReverseMap();

            // LoverLog
            CreateMap<LoverLog, LoverLogResource>();
            CreateMap<LoverLogAddResource, LoverLog>();
            CreateMap<LoverLogUpdateResource, LoverLog>()
                .ReverseMap();

            // LoverAnniversary
            CreateMap<LoverAnniversary, LoverAnniversaryResource>();
            CreateMap<LoverAnniversaryAddResource, LoverAnniversary>();
            CreateMap<LoverAnniversary, LoverAnniversaryUpdateResource>()
                .ReverseMap();

            // Menstruation
            CreateMap<MenstruationDescription, MenstruationDescriptionResource>();
            CreateMap<MenstruationLog, MenstruationLogResource>();

            // LoverCloudUser
            CreateMap<LoverCloudUserUpdateResource, LoverCloudUser>()
                .ReverseMap();
            CreateMap<LoverCloudUser, LoverCloudUserResource>()
                .ForMember(x => x.Sex, c => c.MapFrom(y => y.Sex.GetDescription()))
                .ForMember(x => x.Spouse, c => c.MapFrom(y => y.GetSpouse()));
            CreateMap<LoverCloudUserAddResource, LoverCloudUser>();

            // LoverRequest
            CreateMap<LoverRequestAddResource, LoverRequest>();
            CreateMap<LoverRequest, LoverRequestResource>();
            CreateMap<LoverRequest, LoverRequestUpdateResource>()
                .ReverseMap();
        }
    }
}
