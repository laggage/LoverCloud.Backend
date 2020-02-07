namespace LoverCloud.Infrastructure.Resources
{
    using FluentValidation;
    using LoverCloud.Core.Models;
    using System.Collections.Generic;

    public class LoverAlbumResource : Resource
    {
        /// <summary>
        /// 相册名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 相册描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 相册创建日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 相册中的照片
        /// </summary>
        public IList<LoverPhotoResource> Photos { get; set; }
        public IList<TagResource> Tags { get; set; }
    }

    public class LoverAlbumAddResource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TagAddResource> Tags { get; set; }
    }

    public class LoverAlbumUpdateResource : LoverAlbumAddResource
    {
    }

    public class LoverAlbumAddResourceValidator : AbstractValidator<LoverAlbumAddResource>
    {
        public LoverAlbumAddResourceValidator()
        {
            RuleFor(x => x.Name).MaximumLength(LoverAlbum.NameMaxLength)
                .WithName("相册名")
                .WithMessage($"{{PropertyName}}最大长度为{LoverAlbum.NameMaxLength}");
            RuleForEach(x => x.Tags)
                .SetValidator(new TagAddResourceValidator());
        }
    }
}
