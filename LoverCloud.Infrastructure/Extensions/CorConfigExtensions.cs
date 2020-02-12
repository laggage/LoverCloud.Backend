namespace LoverCloud.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;

    public static class CorConfigExtensions
    {
        public static void AddLoverCloudCors(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddCors(options => ConfigCors(options, configuration));
        }

        public static void UseLoverCloudCors(
            this IApplicationBuilder app, 
            IConfiguration configuration)
        {
            app.UseCors(configuration["CorSettings:PolicyName"]);
        }

        private static void ConfigCors(CorsOptions options, IConfiguration configuration)
        {
            var corSettings = new
            {
                Origins = new List<string>(),
                ExposedHeaders = new List<string>()
            };
            var sec = configuration.GetSection("CorSettings");
            sec.Bind(corSettings);
            options.AddPolicy(sec["PolicyName"], config =>
            {
                config.WithOrigins(corSettings.Origins.ToArray())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders(corSettings.ExposedHeaders.ToArray());
            });
        }
    }
}
