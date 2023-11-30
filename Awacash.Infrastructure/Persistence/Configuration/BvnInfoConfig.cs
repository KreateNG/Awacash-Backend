using System;
using Awacash.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awacash.Infrastructure.Persistence.Configuration
{
    public class BvnInfoConfig : IEntityTypeConfiguration<BvnInfo>
    {
        public void Configure(EntityTypeBuilder<BvnInfo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(250);

            builder.Property(x => x.CreatedByIp).HasMaxLength(250);

            builder.Property(x => x.CreatedDate).IsRequired();

            builder.Property(x => x.ModifiedBy).HasMaxLength(250);

            builder.Property(x => x.ModifiedDate).HasMaxLength(250);

            builder.Property(x => x.AccountDetailId);
            builder.Property(x => x.BranchName);
            builder.Property(x => x.DateOfBirth);
            builder.Property(x => x.Email);
            builder.Property(x => x.EnrollBankCode);
            builder.Property(x => x.EnrollmentDate);
            builder.Property(x => x.FirstName);
            builder.Property(x => x.Gender);
            builder.Property(x => x.ImageDetailsId);
            builder.Property(x => x.LgaOfCapture);
            builder.Property(x => x.LgaOfOrigin);
            builder.Property(x => x.LgaOfResidence);
            builder.Property(x => x.MaritalStatus);
            builder.Property(x => x.MiddleName);
            builder.Property(x => x.NameOnCard);
            builder.Property(x => x.Nationality);
            builder.Property(x => x.Nin);
            builder.Property(x => x.PhoneNumber1);
            builder.Property(x => x.PhoneNumber2);
            builder.Property(x => x.StateOfCapture);
            builder.Property(x => x.StateOfOrigin);
            builder.Property(x => x.StateOfResidence);
            builder.Property(x => x.Surname);
            builder.Property(x => x.Title);

        }
    }
}

