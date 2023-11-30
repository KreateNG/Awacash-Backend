using Awacash.Application.Common.Interfaces.Authentication;
using Awacash.Application.Common.Interfaces.Services;
using Awacash.Domain.Interfaces;
using Awacash.Infrastructure.Authentication;
using Awacash.Infrastructure.Authentication.Extentions;
using Awacash.Infrastructure.Helpers;
using Awacash.Infrastructure.Middlewares;
using Awacash.Infrastructure.Providers.BerachahThirdParty;
using Awacash.Infrastructure.Services;
using Awacash.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            services.Configure<BerachahThirdPartySettings>(configuration.GetSection(BerachahThirdPartySettings.SectionName));

            services.AddScoped<ExceptionMiddleware>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<ICryptoService, CryptoService>();
            services.AddSingleton<IRestSharpHelper, RestSharpHelper>();
            services.AddTransient<IAwacashThirdPartyService, BerachahThirdPartyService>();
            services.AddTransient<IBankOneAccountService, BankOneAccountService>();
            services.AddTransient<IEasyPayService, EasyPayService>();
            services.AddTransient<ILoanProviderService, LoanProviderService>();
            services.AddAuth(configuration);
            return services;
        }
    }
}
