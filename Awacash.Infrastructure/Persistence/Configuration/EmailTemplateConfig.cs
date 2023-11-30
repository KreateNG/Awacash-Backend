using System;
using Awacash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awacash.Infrastructure.Persistence.Configuration
{
    public class EmailTemplateConfig : IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Subject).IsRequired();
            builder.Property(x => x.Body).IsRequired();
            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(250);

            builder.Property(x => x.CreatedByIp).HasMaxLength(250);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.ModifiedBy).HasMaxLength(250);

            builder.Property(x => x.ModifiedDate).HasMaxLength(250);
            builder.Property(x => x.EmailType)
               .HasMaxLength(50)
               .HasConversion<string>();
        }
    }
}

