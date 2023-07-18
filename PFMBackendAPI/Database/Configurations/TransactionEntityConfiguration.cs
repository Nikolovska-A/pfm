using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Configurations
{
    public class TransactionEntityConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public TransactionEntityConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(x => x.TransactionId);
            builder.Property(x => x.TransactionId).IsRequired();
            builder.Property(x => x.BeneficiaryName);
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Direction).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Currency).HasConversion<string>().IsRequired();
            builder.Property(x => x.Mcc);
            builder.Property(x => x.Kind).IsRequired();

        }
    }
}