namespace LoverCloud.Infrastructure.Resources
{
    using FluentValidation;
    using LoverCloud.Core.Models;
    using System;
    using System.Collections.Generic;

    public class LoverLogResource : Resource
    {
        public string Content { get; set; }
        public DateTime CreateDateTime { get; set; }
        public IList<LoverPhotoResource> LoverPhotos { get; set; }
    }

    public class LoverLogAddResource
    {
        /// <summary>
        /// 最大长度: 1024
        /// </summary>
        public string Content { get; set; }
        public IList<string> LoverPhotosId { get; set; }
    }

    public class LoverLogUpdateResource : LoverLogAddResource
    {
    }

    public class LoverLogAddResourceValidator:AbstractValidator<LoverLogAddResource>
    {
        public LoverLogAddResourceValidator()
        {
            RuleFor(x => x.Content)
                .MaximumLength(LoverLog.ContentMaxLength);
        }
    }
}
