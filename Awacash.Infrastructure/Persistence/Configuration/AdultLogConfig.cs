using System;
using Awacash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awacash.Infrastructure.Persistence.Configuration
{
    public class AdultLogConfig : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(250);

            builder.Property(x => x.CreatedByIp).HasMaxLength(250);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.ModifiedBy).HasMaxLength(250);

            builder.Property(x => x.ModifiedDate).HasMaxLength(250);

            builder.Property(x => x.TableName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.EventDate).IsRequired();
            builder.Property(x => x.EventType).IsRequired().HasMaxLength(250);
            builder.Property(x => x.IPAddress).IsRequired().HasMaxLength(20);
            builder.Property(x => x.NewValues).IsRequired().HasMaxLength(5000);
            builder.Property(x => x.OldValues).IsRequired().HasMaxLength(5000);
            builder.Property(x => x.KeyValues).IsRequired().HasMaxLength(500);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(500);

        }
    }
}

