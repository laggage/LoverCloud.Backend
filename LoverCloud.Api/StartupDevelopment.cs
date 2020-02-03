namespace LoverCloud.Api
{
    using AutoMapper;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using LoverCloud.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Reflection;
    using System.Text;

    public class StartupDevelopment
    {
        private readonly IConfiguration _configuration;

        public StartupDevelopment(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 配置数据库
            string connString = LoverCloudDbContext.GetConnectionStringFromFile(
                _configuration.GetConnectionString("MySql"));
            services.AddDbContext<LoverCloudDbContext>(
                config => config.UseMySql(connString));

            // 配置身份认证
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options => _configuration.Bind("JwtSettings", options));
            services.AddIdentityCore<LoverCloudUser>(options =>
                {
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                    options.User.AllowedUserNameCharacters = string.Empty;
                })
                .AddEntityFrameworkStores<LoverCloudDbContext>();

            services.AddRepositories();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddCors(options =>
            {
                var corSettings = new
                {
                    Origins = new List<string>(),
                    ExposedHeaders = new List<string>()
                };
                var sec = _configuration.GetSection("CorSettings");
                sec.Bind(corSettings);
                options.AddPolicy(sec["PolicyName"], config =>
                {
                    config.WithOrigins(corSettings.Origins.ToArray())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders(corSettings.ExposedHeaders.ToArray());
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseCors(_configuration["CorSettings:PolicyName"]);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
