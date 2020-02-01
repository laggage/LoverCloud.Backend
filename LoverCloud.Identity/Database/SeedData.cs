namespace LoverCloud.Identity.Database
{
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Mappers;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using IdentityServer4.Models;
    using IdentityServer4.Test;
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
            }

            foreach (ApiResource apiResource in Config.Apis)
            {
                var old = configurationDbContext.ApiResources.FirstOrDefault(x => x.Name == apiResource.Name);
                if (old != null) configurationDbContext.ApiResources.Remove(old);
                configurationDbContext.ApiResources.Add(apiResource.ToEntity());
            }

            foreach (IdentityResource identityResource in Config.Ids)
            {
                var old = configurationDbContext.IdentityResources.FirstOrDefault(x => x.Name == identityResource.Name);
                if (old != null) configurationDbContext.IdentityResources.Remove(old);
                configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
            }

            configurationDbContext.SaveChanges();
        }

        public static async Task EnsureSeedData(UserManager<LoverCloudUser> userManager)
        {
            foreach (TestUser testUser in Config.TestUsers)
            {
                // 删除重复
                if (await userManager.FindByNameAsync(
                    testUser.Username) is LoverCloudUser userExist)
                {
                    await userManager.DeleteAsync(userExist);
                }

                var result = await userManager.CreateAsync(
                    new LoverCloudUser(testUser.Username), testUser.Password);
                if (!result.Succeeded)
                {
                    Log.Error(
                        $"Failed to Create user. {result.Errors.First()?.Description}");
                }
                else
                {
                    var user = await userManager.FindByNameAsync(testUser.Username);

                    var claims = testUser.Claims.ToList();
                    var roleClaim = claims.FirstOrDefault(
                        x => x.Type.Equals(
                            "role", StringComparison.OrdinalIgnoreCase));
                    if (roleClaim != null) claims.Remove(roleClaim);

                    var role = roleClaim?.Value;
                    if (!string.IsNullOrEmpty(role)) await userManager.AddToRoleAsync(user, role);
                    await userManager.AddClaimsAsync(user, claims);
                }
            }
        }
    }
}
