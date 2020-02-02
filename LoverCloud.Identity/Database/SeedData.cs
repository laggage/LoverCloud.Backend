namespace LoverCloud.Identity.Database
{
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Mappers;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using IdentityServer4.Models;
    using LoverCloud.Core.Models;
    using Microsoft.AspNetCore.Identity;
    using Serilog;
    using System.Threading.Tasks;

    public static class SeedData
    {
        public static void EnsureSeedData(IServiceProvider services)
        {
            
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;
            Log.Information("Begin seeding data");
            try
            {
                var configurationDbContext = sp.GetRequiredService<ConfigurationDbContext>();
                var userManager = sp.GetRequiredService<UserManager<LoverCloudUser>>();
                EnsureSeedData(configurationDbContext); 
                EnsureSeedData(userManager).Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Encounter error while seeding data");
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext configurationDbContext)
        {
            foreach (var client in Config.Clients)
            {
                var old = configurationDbContext.Clients.FirstOrDefault(x => x.ClientId == client.ClientId);
                if (old != null)
                    configurationDbContext.Clients.Remove(old);
                configurationDbContext.Clients.Add(client.ToEntity());

                Log.Information($"Client definition find, clientName: {client.ClientName}");
            }

            foreach (ApiResource apiResource in Config.Apis)
            {
                var old = configurationDbContext.ApiResources.FirstOrDefault(x => x.Name == apiResource.Name);
                if (old != null) configurationDbContext.ApiResources.Remove(old);
                configurationDbContext.ApiResources.Add(apiResource.ToEntity());

                Log.Information($"ApiResource definition find, api resource display name: {apiResource.DisplayName}");

            }

            foreach (IdentityResource identityResource in Config.Ids)
            {
                var old = configurationDbContext.IdentityResources.FirstOrDefault(x => x.Name == identityResource.Name);
                if (old != null) configurationDbContext.IdentityResources.Remove(old);
                configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
                Log.Information($"IdentityResource definition find, identity resource display name: {identityResource.DisplayName}");
            }

            Log.Information($"Seed ConfigruationDbContext successed, seeded data numbers: {configurationDbContext.SaveChanges().ToString()}");
        }

        public static async Task EnsureSeedData(UserManager<LoverCloudUser> userManager)
        {
            string pwd = "123456789";
            foreach (var user in Config.Users)
            {
                // 删除重复
                if (await userManager.FindByNameAsync(
                    user.UserName) is LoverCloudUser userExist)
                {
                    await userManager.DeleteAsync(userExist);
                }

                var result = await userManager.CreateAsync(
                    user, pwd);
                if (!result.Succeeded)
                    Log.Error(
                        $"Failed to Create user. {result.Errors.First()?.Description}");
                else
                    Log.Information($"Create user {user.ToString()}");
            }

            Log.Information($"Seed LoverCloudDbContext successed");
        }
    }
}
