namespace LoverCloud.Infrastructure.Resources
{
    using FluentValidation;
    using LoverCloud.Core.Models;
    using System;

    public class TagResource : Resource
    {
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class TagAddResource
    {
        public string Name { get; set; }
    }

    public class TagAddResourceValidator : AbstractValidator<TagAddResource>
    {
        public TagAddResourceValidator()
        {
            RuleFor(x => x.Name).MaximumLength(Tag.NameMaxLength)
                .WithName("标签")
                .WithMessage($"{{PropertyName}}的最大长度是{Tag.NameMaxLength.ToString()}")
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName}不能为空");
        }
    }
}
