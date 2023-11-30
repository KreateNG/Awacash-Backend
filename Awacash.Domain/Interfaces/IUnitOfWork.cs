using Awacash.Domain.Entities;
using Awacash.Domain.IdentityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAsyncRepository<ApplicationRole, string> ApplicationRoleRepository { get; set; }
        IAsyncRepository<ApplicationRoleClaim, string> ApplicationRoleClaimRepository { get; set; }
        IAsyncRepository<ApplicationUser, string> ApplicationUserRepository { get; set; }
        IAsyncRepository<ApplicationUserRole, string> ApplicationUserRoleRepository { get; set; }
        IAsyncRepository<Customer, string> CustomerRepository { get; set; }
        IAsyncRepository<Token, string> TokenRepository { get; set; }
        IAsyncRepository<Wallet, string> WalletRepository { get; set; }
        IAsyncRepository<Transaction, string> TransactionRepository { get; set; }
        IAsyncRepository<AccountTransactionNotification, string> AccountTransactionNotificationRepository { get; set; }
        IAsyncRepository<Saving, string> SavingRepository { get; set; }
        IAsyncRepository<SavingConfiguration, string> SavingConfigurationRepository { get; set; }
        IAsyncRepository<Promotion, string> PromotionRepository { get; set; }
        IAsyncRepository<CardRequest, string> CardRequestRepository { get; set; }
        IAsyncRepository<CardRequestConfiguration, string> CardRequestConfigurationtRepository { get; set; }
        IAsyncRepository<Beneficiary, string> BeneficiaryRepository { get; set; }
        IAsyncRepository<DisputeLog, string> DispusteLogRepository { get; set; }
        IAsyncRepository<FeeConfiguration, string> FeeConfigurationRepository { get; set; }
        IAsyncRepository<SmsTemplate, string> SmsTemplateRepository { get; set; }
        IAsyncRepository<EmailTemplate, string> EmailTemplateRepository { get; set; }
        IAsyncRepository<AuditLog, string> AuditLogRepository { get; set; }
        IAsyncRepository<BvnInfo, string> BvnInfoRepository { get; set; }
        IAsyncRepository<Document, string> DocumentRepository { get; set; }
        IAsyncRepository<Referral, string> ReferralRepository { get; set; }


        Task<int> Complete();
    }
}
