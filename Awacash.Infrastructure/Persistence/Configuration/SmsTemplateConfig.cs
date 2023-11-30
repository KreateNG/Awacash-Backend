using System;
using Awacash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awacash.Infrastructure.Persistence.Configuration
{
	public class SmsTemplateConfig: IEntityTypeConfiguration<SmsTemplate>
    {
        public void Configure(EntityTypeBuilder<SmsTemplate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Message).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(250);

            builder.Property(x => x.CreatedByIp).HasMaxLength(250);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.ModifiedBy).HasMaxLength(250);

            builder.Property(x => x.ModifiedDate).HasMaxLength(250);
            builder.Property(x => x.SmsType)
               .HasMaxLength(50)
               .HasConversion<string>();
        }
    }
}

