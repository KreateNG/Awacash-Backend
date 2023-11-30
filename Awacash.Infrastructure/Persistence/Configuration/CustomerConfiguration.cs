using Awacash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awacash.Infrastructure.Persistence.Configuration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(250);

            builder.Property(x => x.CreatedByIp).HasMaxLength(250);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.ModifiedBy).HasMaxLength(250);

            builder.Property(x => x.ModifiedDate).HasMaxLength(250);

            builder.Property(x => x.FirstName).HasMaxLength(250);

            builder.Property(x => x.LastName).HasMaxLength(250);

            builder.Property(x => x.MiddleName).HasMaxLength(250);

            builder.Property(x => x.PhoneNumber).HasMaxLength(250);

            builder.Property(x => x.Email).HasMaxLength(250);

            builder.Property(x => x.SaltedHashedPassword).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.SaltedHashedPin).IsRequired().HasMaxLength(1000);
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.PhoneNumber).IsUnique();
            //builder.HasMany(x => x.Referrals)
            //    .WithOne(x => x.Referrer)
            //    .HasForeignKey(r => r.ReferrerId);

        }
    }
}
