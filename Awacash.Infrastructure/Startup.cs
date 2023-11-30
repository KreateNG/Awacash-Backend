using Awacash.Infrastructure.Authentication.Extentions;
using Awacash.Infrastructure.Filters;
using Awacash.Infrastructure.Middlewares;
using Awacash.Infrastructure.OpenApi;
using Awacash.Infrastructure.Persistence;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Awacash.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddApiVersioning()
                .AddOpenApiDocumentation(config)
                .AddPersistence(config)
                .AddServices(config)

                .AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(config.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer(); ;
            return services;
        }

        internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
       app.UseMiddleware<ExceptionMiddleware>();


        private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.DefaultApiVersion = new ApiVersion(1, 0);
            });
        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
            builder

                .UseStaticFiles()
                .UseExceptionMiddleware()
                //.UseExceptionHandler("/error")
                .UseRouting()
                //.UseMiddleware<NullTenantMiddleware>()
                .UseCors("corsapp")
                .UseAuthentication()
                .UseCurrentUser()
                .UseOpenApiDocumentation(config)
            .UseAuthorization()

            .UseHangfireDashboard("/sysjobs", new DashboardOptions
            {
                Authorization = new[] { new DashboardAuthorizationFilter() }
            })
            .UseEndpoints(config => config.MapControllers());


    }
}
