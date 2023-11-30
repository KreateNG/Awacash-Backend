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
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedBy).HasMaxLength(250);

            builder.Property(x => x.CreatedByIp).HasMaxLength(250);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.ModifiedBy).HasMaxLength(250);

            builder.Property(x => x.ModifiedDate).HasMaxLength(250);
            builder.Property(x => x.Status)
               .HasMaxLength(50)
               .HasConversion<string>();
            builder.Property(x => x.TransactionType)
               .HasMaxLength(50)
               .HasConversion<string>();
            builder.Property(x => x.RecordType)
               .HasMaxLength(50)
               .HasConversion<string>();
        }
    }
}
