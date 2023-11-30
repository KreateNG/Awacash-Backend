using Awacash.Application.Authentication.Services;
using Awacash.Application.Beneficiaries.Services;
using Awacash.Application.BillPayment.Services;
using Awacash.Application.CardRequests.Services;
using Awacash.Application.CardRequestConfigurations.Services;
using Awacash.Application.Common.Behaviors;
using Awacash.Application.Customers.Services;
using Awacash.Application.Promotions.Services;
using Awacash.Application.Role.Services;
using Awacash.Application.Savings.Services;
using Awacash.Application.SavingsConfiguration.Services;
using Awacash.Application.Transactions.Services;
using Awacash.Application.Transfers.Services;
using Awacash.Application.Users.Services;
using Awacash.Application.Wema.Services;
using Awacash.Domain.Settings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Awacash.Application.DisputeLogs.Services;
using Awacash.Application.FeeConfigurations.Services;
using Awacash.Application.SmsTemplateConfigurations.Services;
using Awacash.Application.EmailTemplateConfigurations.Services;
using Awacash.Application.Wallets.Services;
using Awacash.Application.AuditLogs.Services;
using Awacash.Application.DashBoard.Services;
using Awacash.Application.Documents.Services;
using AwaCash.Application.Common.Interfaces.Services;
using Awacash.Application.Loans.Services;

namespace Awacash.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.Configure<AppSettings>(configuration.GetSection(AppSettings.SectionName));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ITransferService, TransferService>();
            services.AddTransient<IWemaService, WemaService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ISavingService, SavingService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBillPaymentService, BillPaymentService>();
            services.AddTransient<ISavingConfigurationService, SavingConfigurationService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddTransient<IBeneficiaryService, BeneficiaryService>();
            services.AddTransient<ICardRequestConfigurationService, CardRequestConfigurationService>();
            services.AddTransient<ICardRequestService, CardRequestService>();
            services.AddTransient<IDisputeLogService, DisputeLogService>();
            services.AddTransient<IFeeConfigurationService, FeeConfigurationService>();
            services.AddTransient<ISmsConfigurationService, SmsConfigurationService>();
            services.AddTransient<IEmailConfigurationService, EmailConfigurationService>();
            services.AddTransient<IWalletService, WalletService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAuditLogService, AuditLogService>();
            services.AddTransient<IDashBoardService, DashBoardService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IOtpService, OtpService>();
            services.AddTransient<ILoanService, LoanService>();


            return services;
        }
    }
}
