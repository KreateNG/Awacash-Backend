using Audit.EntityFramework;
using Awacash.Domain.Entities;
using Awacash.Domain.IdentityModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : AuditIdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //options.
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<AccountTransactionNotification> AccountTransactions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Saving> Savings { get; set; }
        public DbSet<SavingConfiguration> SavingsConfigurations { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<CardRequest> CardRequests { get; set; }
        public DbSet<CardRequestConfiguration> CardRequestConfigurations { get; set; }
        public DbSet<Beneficiary> Beneficiary { get; set; }
        public DbSet<FeeConfiguration> FeeConfigurations { get; set; }
        public DbSet<DisputeLog> DisputeLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Referral> Referrals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Entity<Customer>().HasIndex(x => x.Email).IsUnique();
            //modelBuilder.Entity<Customer>().HasIndex(x => x.PhoneNumber).IsUnique();
            //modelBuilder.Entity<Beneficiary>().Property(x => x.Email).IsUnique();

        }
    }

}
