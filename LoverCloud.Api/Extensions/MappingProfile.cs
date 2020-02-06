namespace LoverCloud.Api.Extensions
{
    using AutoMapper;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Extensions;
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


            CreateMap<LoverCloudUserUpdateResource, LoverCloudUser>()
                .ReverseMap();

            CreateMap<LoverCloudUser, LoverCloudUserResource>()
                .ForMember(x => x.Guid, c => c.MapFrom(s => s.Id))
                .ForMember(x => x.Sex, c => c.MapFrom(y => y.Sex.GetDescription()))
                .ForMember(x => x.Spouse, c => c.MapFrom(y => y.GetSpouse()));

            CreateMap<LoverCloudUserAddResource, LoverCloudUser>()
                .ForMember(m => m.Sex, c => c.MapFrom(x => x.Sex == "男" ? Sex.Male : Sex.Female));

            CreateMap<LoverRequestAddResource, LoverRequest>();
            CreateMap<LoverRequest, LoverRequestResource>();

            CreateMap<LoverRequest, LoverRequestUpdateResource>();
            CreateMap<LoverRequestUpdateResource, LoverRequest>();

        }
    }
}
