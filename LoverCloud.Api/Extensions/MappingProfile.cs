﻿namespace LoverCloud.Api.Extensions
{
    using AutoMapper;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.Mvc;

    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Lover
            CreateMap<Lover, LoverResource>();
            CreateMap<LoverUpdateResource, Lover>()
                .ReverseMap();

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
            CreateMap<LoverPhoto, LoverPhotoResource>()
                .ForMember(x => x.Url, c => c.MapFrom(x => x.PhotoUrl));
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
                //.ForMember(x => x.Sex, c => c.MapFrom(y => y.Sex.ToString("d")))
                .ForMember(x => x.Spouse, c => c.MapFrom(y => y.GetSpouse()));
            CreateMap<LoverCloudUserAddResource, LoverCloudUser>();

            // LoverRequest
            CreateMap<LoverRequestAddResource, LoverRequest>();
            CreateMap<LoverRequest, LoverRequestResource>();
            CreateMap<LoverRequest, LoverRequestUpdateResource>()
                .ReverseMap();

            // Menstruation
            CreateMap<MenstruationLog, MenstruationLogResource>();
            CreateMap<MenstruationLogAddResource, MenstruationLog>();
            CreateMap<MenstruationLogUpdateResource, MenstruationLog>()
                .ReverseMap();

            CreateMap<MenstruationDescription, MenstruationDescriptionResource>();
            CreateMap<MenstruationDescriptionAddResource, MenstruationDescription>();
            CreateMap<MenstruationDescriptionUpdateResource, MenstruationDescription>()
                .ReverseMap();
        }
    }
}
