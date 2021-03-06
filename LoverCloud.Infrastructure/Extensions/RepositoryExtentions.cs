﻿namespace LoverCloud.Infrastructure.Extensions
{
    using FluentValidation;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Infrastructure.Repositories;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.Extensions.DependencyInjection;

    public static class RepositoryExtentions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ILoverRepository, LoverRepository>();
            services.AddScoped<ILoverCloudUserRepository, LoverCloudUserRepository>();
            services.AddScoped<ILoverRequestRepository, LoverRequestRepository>();
            services.AddScoped<ILoverLogRepository, LoverLogRepository>();
            services.AddScoped<ILoverPhotoRepository, LoverPhotoRepository>();
            services.AddScoped<ILoverAlbumRepository, LoverAlbumRepository>();
            services.AddScoped<ILoverAnniversaryRepository, LoverAnniversaryRepository>();
            services.AddScoped<IMenstruationLogRepository, MenstruationLogRepository>();
            services.AddScoped<IMenstruationDescriptionRepository, MenstruationDescriptionRepository>();

            services.AddTransient<IValidator<LoverAlbumAddResource>, LoverAlbumAddResourceValidator>();
            services.AddTransient<IValidator<LoverPhotoAddResource>, LoverPhotoAddResourceValidator>();
            services.AddTransient<IValidator<TagAddResource>, TagAddResourceValidator>();
            services.AddTransient<IValidator<LoverAnniversaryAddResource>, LoverAnniversaryAddResourceValidator>();
            services.AddTransient<IValidator<MenstruationLogAddResource>, MenstruationLogAddResourceValidator>();
            services.AddTransient<IValidator<MenstruationDescriptionAddResource>, MenstruationDescriptionAddResourceValidator>();

        }
    }
}
