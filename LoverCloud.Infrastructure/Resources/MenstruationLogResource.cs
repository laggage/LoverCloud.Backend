namespace LoverCloud.Infrastructure.Resources
{
    using FluentValidation;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Services;
    using System;
    using System.Collections.Generic;

    public class MenstruationDescriptionResource : Resource
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class MenstruationDescriptionAddResource
    {
        public string Description { get; set; }
    }

    public class MenstruationDescriptionUpdateResource : MenstruationDescriptionAddResource
    {
    }

    public class MenstruationLogResource : Resource
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IList<MenstruationDescriptionResource> MenstruationDescriptions { get; set; }
    }

    public class MenstruationLogAddResource 
    {
        /// <summary>
        /// 表示姨妈开始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 表示姨妈结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 姨妈描述集合
        /// 姨妈期间每天可以发表多个的姨妈描述
        /// </summary>
        public IList<MenstruationDescriptionAddResource> MenstruationDescriptions { get; set; }
    }

    public class MenstruationLogUpdateResource : MenstruationLogAddResource
    {
    }

    public class MenstruationLogAddResourceValidator : AbstractValidator<MenstruationLogAddResource>
    {
        public MenstruationLogAddResourceValidator()
        {
            RuleFor(x => x.StartDate)
                .NotNull();
            RuleFor(x => x.EndDate)
                .NotNull();
            RuleForEach(x => x.MenstruationDescriptions)
                .SetValidator(new MenstruationDescriptionAddResourceValidator());
        }
    }

    public class MenstruationDescriptionAddResourceValidator : AbstractValidator<MenstruationDescriptionAddResource>
    {
        public MenstruationDescriptionAddResourceValidator()
        {
            RuleFor(x => x.Description)
                .MaximumLength(MenstruationDescription.DescriptionMaxLength);
        }
    }

    public class MenstruationLogResourceMapping : PropertyMapping<MenstruationLogResource, MenstruationLog>
    {
        public MenstruationLogResourceMapping()
            :base(new Dictionary<string, IList<MappedProperty>>
            {
                {
                    nameof(MenstruationLogResource.StartDate),
                    new List<MappedProperty>
                    {
                        new MappedProperty(nameof(MenstruationLog.StartDate))
                    }
                },
                {
                    nameof(MenstruationLogResource.EndDate),
                    new List<MappedProperty>
                    {
                        new MappedProperty(nameof(MenstruationLog.EndDate))
                    }
                },
            })
        {
        }
    }
}
