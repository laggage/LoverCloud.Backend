namespace LoverCloud.Identity
{
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using LoverCloud.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Reflection;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connString = LoverCloudDbContext.GetConnectionStringFromFile(
                _configuration.GetConnectionString("MySql"));
            string migrationAssembly = Assembly.GetExecutingAssembly().FullName;

            services.AddDbContext<LoverCloudDbContext>(config =>
            {
                config.UseMySql(
                    connString, sql => sql.MigrationsAssembly(migrationAssembly));
            });

            services.AddIdentity<LoverCloudUser, IdentityRole>(options =>
                {
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                    options.User.AllowedUserNameCharacters = string.Empty;
                })
                .AddEntityFrameworkStores<LoverCloudDbContext>()
                .AddDefaultTokenProviders();


            var builder = services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = x =>
                    {
                        x.UseMySql(
                            connString, sql => sql.MigrationsAssembly(migrationAssembly));
                    };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = x =>
                        x.UseMySql(
                            connString, sql => sql.MigrationsAssembly(migrationAssembly));
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<LoverCloudUser>();

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddLoverCloudCors(_configuration);

        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseLoverCloudCors(_configuration);
            // uncomment if you want to add MVC
            //app.UseStaticFiles();
            //app.UseRouting();

            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapDefaultControllerRoute();
            //});
        }
    }
}
