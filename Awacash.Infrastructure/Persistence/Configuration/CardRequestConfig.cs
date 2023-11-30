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
    public class CardRequestConfig : IEntityTypeConfiguration<CardRequest>
    {
        public void Configure(EntityTypeBuilder<CardRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(250);

            builder.Property(x => x.CreatedByIp).HasMaxLength(250);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.ModifiedBy).HasMaxLength(250);

            builder.Property(x => x.ModifiedDate).HasMaxLength(250);
            builder.Property(x => x.CardType)
               .HasMaxLength(50)
               .HasConversion<string>();
            builder.Property(x => x.DeliveryStatus)
               .HasMaxLength(50)
               .HasConversion<string>();
        }
    }
}
