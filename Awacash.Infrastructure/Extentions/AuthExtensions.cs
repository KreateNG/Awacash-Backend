//using Awacash.Application.Common.Interfaces.Authentication;
//using Awacash.Infrastructure.Authentication;
//using Awacash.Infrastructure.Authentication.Extentions;
//using Awacash.Infrastructure.Identity.Extentions;
//using Awacash.Infrastructure.Middlewares;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Awacash.Infrastructure.Extentions
//{
//    internal static class AuthExtensions
//    {
//        internal static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
//        {
//            services
//                .AddCurrentUser()
//                .AddPermissions()

//                // Must add identity before adding auth!
//                .AddIdentity();

//            return services.AddJwtAuth(config);
//        }

//        internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app)
//        {
//            app.UseMiddleware<CurrentUserMiddleware>();

//            return app;
//        }

//        private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
//            services
//                .AddScoped<CurrentUserMiddleware>()
//                .AddScoped<ICurrentUser, CurrentUser>()
//                .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());

//        private static IServiceCollection AddPermissions(this IServiceCollection services) =>
//            services
//                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
//                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
//    }
//}
//}
