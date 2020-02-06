namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;

    public class LoverCloudUserManager : UserManager<LoverCloudUser>
    {
        public LoverCloudUserManager(IUserStore<LoverCloudUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<LoverCloudUser> passwordHasher,
            IEnumerable<IUserValidator<LoverCloudUser>> userValidators,
            IEnumerable<IPasswordValidator<LoverCloudUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<LoverCloudUser>> logger)
            : base(store, optionsAccessor, 
                passwordHasher, userValidators, 
                passwordValidators,keyNormalizer, 
                errors, services, logger)
        {
        }
    }
}
