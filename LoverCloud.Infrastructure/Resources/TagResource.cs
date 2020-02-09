namespace LoverCloud.Infrastructure.Resources
{
    using FluentValidation;
    using LoverCloud.Core.Models;
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class TagResource : Resource
    {
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class TagAddResource : IEquatable<TagAddResource>
    {
        public TagAddResource()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Name { get; set; }

        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as LoverAlbumUpdateResource);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool Equals([AllowNull] TagAddResource other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }

        public static bool operator == (TagAddResource x, TagAddResource y)
        {
            if (ReferenceEquals(x, y))
                return true;
            return x.Equals(y);
        }
        public static bool operator !=(TagAddResource x, TagAddResource y)
        {
            if (ReferenceEquals(x, y))
                return true;
            return !x.Equals(y);
        }
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
