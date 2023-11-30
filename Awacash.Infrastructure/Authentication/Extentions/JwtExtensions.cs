using Awacash.Application.Common.Interfaces.Authentication;
using AwaCash.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Awacash.Infrastructure.Authentication.Extentions
{
    internal static class JwtExtensions
    {
        internal static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
        {
            var jwtSettings = new JwtSettings();
            config.Bind(JwtSettings.SectionName, jwtSettings);
            services.AddSingleton(Options.Create(jwtSettings));



            //services.Configure<JwtSettings>(config.GetSection($"SecuritySettings:{nameof(JwtSettings)}"));
            //var jwtSettings = config.GetSection($"SecuritySettings:{nameof(JwtSettings)}").Get<JwtSettings>();


            if (string.IsNullOrEmpty(jwtSettings.Secret))
                throw new InvalidOperationException("No Key defined in JwtSettings config.");
            byte[] key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            return services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        //RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };
                    bearer.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                throw new UnauthorizedException("Authentication Failed.");

                            }

                            return Task.CompletedTask;
                        },
                        OnForbidden = _ => throw new ForbiddenException("You are not authorized to access this resource."),
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            if (!string.IsNullOrEmpty(accessToken) &&
                                context.HttpContext.Request.Path.StartsWithSegments("/notifications"))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                })
                .Services;
        }
    }
}
