namespace LoverCloud.Infrastructure.Resources
{
    using FluentValidation;
    using LoverCloud.Core.Models;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;

    public class LoverPhotoResource : Resource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime PhotoTakenDate { get; set; }
        public LoverAlbumResource Album { get; set; }
        public IList<TagResource> Tags { get; set; }
    }

    public class LoverPhotoAddResource : LoverPhotoUpdateResource
    {
        /// <summary>
        /// 照片文件
        /// </summary>
        public IFormFile File { get; set; }
    }

    public class LoverPhotoUpdateResource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AlbumId { get; set; }
        public IList<TagAddResource> Tags { get; set; }
        /// <summary>
        /// 照片拍摄时间
        /// </summary>
        public DateTime PhotoTakenDate { get; set; }
    }

    public class LoverPhotoAddResourceValidator : AbstractValidator<LoverPhotoAddResource>
    {
        public LoverPhotoAddResourceValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(LoverPhoto.NameMaxLength)
                .WithName("照片名")
                .WithMessage($"{{PropertyName}}的最大长度是{LoverPhoto.NameMaxLength.ToString()}");
        }
    }
}