namespace LoverCloud.Infrastructure.Resources
{
    using FluentValidation;
    using LoverCloud.Core.Models;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;

    public class LoverCloudUserResource : Resource
    {
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }
        public Sex Sex { get; set; }
        public DateTime Birth { get; set; }
        public MenstruationLogResource MenstruationLog { get; set; }
        public LoverCloudUserResource Spouse { get; set; }
        public IList<LoverRequestResource> ReceivedLoverRequests { get; set; }
        public IList<LoverRequestResource> LoverRequests { get; set; }
        /// <summary>
        /// 情侣日志数量
        /// </summary>
        public int LoverLogCount { get; set; }
        /// <summary>
        /// 情侣相册数量
        /// </summary>
        public int LoverAlbumCount { get; set; }
        /// <summary>
        /// 情侣纪念日数量
        /// </summary>
        public int LoverAnniversaryCount { get; set; }
    }

    public class LoverCloudUserAddResource
    {
        public string Password { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// value can be "male" or "female"
        /// </summary>
        public Sex Sex { get; set; }
        public string UserName { get; set; }
        public DateTime Birth { get; set; }
        public IFormFile ProfileImage { get; set; }
    }

    public class LoverCloudUserUpdateResource
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime Birth { get; set; }
        public IFormFile ProfileImage { get; set; }
    }

    public class LoverCoudUserAddResourceValidator : AbstractValidator<LoverCloudUserAddResource>
    {
        public LoverCoudUserAddResourceValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .WithName("邮箱")
                .WithMessage("{PropertyName}是必填的")
                .MaximumLength(LoverCloudUser.EmailMaxLength)
                .WithMessage($"{{PropertyName}}的最大长度为{LoverCloudUser.EmailMaxLength}");
            RuleFor(x => x.UserName)
                .MaximumLength(LoverCloudUser.UserNameMaxLength)
                .WithName("用户名")
                .WithMessage($"{{PropertyName}}的最大长度{LoverCloudUser.UserNameMaxLength}, 你输入了{{TotalLength}}个字符.")
                .NotEmpty()
                .NotNull()
                .WithMessage("{PropertyName}是必填的!");
            RuleFor(x => x.Birth)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.Sex)
                .NotNull()
                .NotEmpty()
                .IsInEnum();
            RuleFor(x => x.ProfileImage)
                .NotNull()
                .ChildRules(
                x => x.RuleFor(file => file.FileName)
                .NotNull()
                .NotEmpty()
                .Matches(@".{1,512}\.(jpg|png|bmp|jpeg|gif)"))
                .WithMessage("文件格式错误, 文件必须是图片文件");
        }
    }
}
