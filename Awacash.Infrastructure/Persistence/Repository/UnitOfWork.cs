using Awacash.Domain.Entities;
using Awacash.Domain.IdentityModel;
using Awacash.Domain.Interfaces;
using Awacash.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IAsyncRepository<Customer, string> CustomerRepository { get; set; }
        public IAsyncRepository<Token, string> TokenRepository { get; set; }
        public IAsyncRepository<Wallet, string> WalletRepository { get; set; }
        public IAsyncRepository<Transaction, string> TransactionRepository { get; set; }
        public IAsyncRepository<ApplicationRole, string> ApplicationRoleRepository { get; set; }
        public IAsyncRepository<ApplicationRoleClaim, string> ApplicationRoleClaimRepository { get; set; }
        public IAsyncRepository<ApplicationUser, string> ApplicationUserRepository { get; set; }
        public IAsyncRepository<ApplicationUserRole, string> ApplicationUserRoleRepository { get; set; }
        public IAsyncRepository<AccountTransactionNotification, string> AccountTransactionNotificationRepository { get; set; }
        public IAsyncRepository<Saving, string> SavingRepository { get; set; }
        public IAsyncRepository<SavingConfiguration, string> SavingConfigurationRepository { get; set; }
        public IAsyncRepository<Promotion, string> PromotionRepository { get; set; }
        public IAsyncRepository<CardRequest, string> CardRequestRepository { get; set; }
        public IAsyncRepository<Beneficiary, string> BeneficiaryRepository { get; set; }
        public IAsyncRepository<DisputeLog, string> DispusteLogRepository { get; set; }
        public IAsyncRepository<CardRequestConfiguration, string> CardRequestConfigurationtRepository { get; set; }
        public IAsyncRepository<FeeConfiguration, string> FeeConfigurationRepository { get; set; }
        public IAsyncRepository<SmsTemplate, string> SmsTemplateRepository { get; set; }
        public IAsyncRepository<EmailTemplate, string> EmailTemplateRepository { get; set; }
        public IAsyncRepository<AuditLog, string> AuditLogRepository { get; set; }
        public IAsyncRepository<BvnInfo, string> BvnInfoRepository { get; set; }
        public IAsyncRepository<Document, string> DocumentRepository { get; set; }
        public IAsyncRepository<Referral, string> ReferralRepository { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CustomerRepository = new EfRepository<Customer, string>(context);
            TokenRepository = new EfRepository<Token, string>(context);
            WalletRepository = new EfRepository<Wallet, string>(context);
            TransactionRepository = new EfRepository<Transaction, string>(context);
            ApplicationRoleRepository = new EfRepository<ApplicationRole, string>(context);
            ApplicationUserRepository = new EfRepository<ApplicationUser, string>(context);
            ApplicationRoleClaimRepository = new EfRepository<ApplicationRoleClaim, string>(context);
            ApplicationUserRoleRepository = new EfRepository<ApplicationUserRole, string>(context);
            AccountTransactionNotificationRepository = new EfRepository<AccountTransactionNotification, string>(context);
            SavingRepository = new EfRepository<Saving, string>(context);
            SavingConfigurationRepository = new EfRepository<SavingConfiguration, string>(context);

            PromotionRepository = new EfRepository<Promotion, string>(context);
            CardRequestRepository = new EfRepository<CardRequest, string>(context);
            BeneficiaryRepository = new EfRepository<Beneficiary, string>(context);
            DispusteLogRepository = new EfRepository<DisputeLog, string>(context);
            CardRequestConfigurationtRepository = new EfRepository<CardRequestConfiguration, string>(context);
            FeeConfigurationRepository = new EfRepository<FeeConfiguration, string>(context);
            SmsTemplateRepository = new EfRepository<SmsTemplate, string>(context);
            EmailTemplateRepository = new EfRepository<EmailTemplate, string>(context);
            AuditLogRepository = new EfRepository<AuditLog, string>(context);
            BvnInfoRepository = new EfRepository<BvnInfo, string>(context);
            DocumentRepository = new EfRepository<Document, string>(context);
            ReferralRepository = new EfRepository<Referral, string>(context);
        }

        public Task<int> Complete()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
