using Awacash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TransactionQueryJob.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<EmailTemplate>().Property(x => x.EmailType)
               .HasMaxLength(50)
               .HasConversion<string>();
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BvnInfo> BvnInfo { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<AccountTransactionNotification> AccountTransactions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<EmailTemplate> EmailTemplate { get; set; }
        public DbSet<Saving> Savings { get; set; }
    }
}
