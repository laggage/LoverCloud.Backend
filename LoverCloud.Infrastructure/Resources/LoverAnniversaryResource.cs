namespace LoverCloud.Infrastructure.Resources
{
    using FluentValidation;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Services;
    using System;
    using System.Collections.Generic;

    public class LoverAnniversaryResource : Resource
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class LoverAnniversaryAddResource
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }

    public class LoverAnniversaryUpdateResource : LoverAnniversaryAddResource
    {
    }

    public class LoverAnniversaryAddResourceValidator : AbstractValidator<LoverAnniversaryAddResource>
    {
        public LoverAnniversaryAddResourceValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(LoverAnniversary.NameMaxLength);
            RuleFor(x => x.Description)
                .MaximumLength(LoverAnniversary.DescriptionMaxLength);
        }
    }

    public class LoverAnniversaryResourceMapping : PropertyMapping<LoverAnniversaryResource, LoverAnniversary>
    {
        public LoverAnniversaryResourceMapping()
            : base(new Dictionary<string, IList<MappedProperty>>
            {
                { 
                    nameof(LoverAnniversaryResource.Name), new MappedProperty[] 
                    {
                        new MappedProperty(nameof(LoverAnniversary.Name))
                    }
                },
                {
                    nameof(LoverAnniversaryResource.Date), new MappedProperty[]
                    {
                        new MappedProperty(nameof(LoverAnniversary.Date))
                    }
                },
            })
        {

        }
    }
}
