namespace LoverCloud.Api
{
    using AutoMapper;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using LoverCloud.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LoverCloudUser", 
                    Version = "v1",
                    Description = "LoverCloud api document powered by swagger.",
                    Contact = new OpenApiContact
                    {
                        Name = "Laggage",
                        Email = "1634205628@qq.com",
                        Url = new Uri("https://www.laggage.top:4201")
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                xmlFile = $"{typeof(IEntity).Assembly.GetName().Name}.xml";
                xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                xmlFile = $"{typeof(LoverCloudDbContext).Assembly.GetName().Name}.xml";
                xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
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
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoverCloud Api V1"); });
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
