using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFMBackendAPI.Database.Entities;

namespace PFMBackendAPI.Database.Configurations
{
	public class SplitEntityConfiguration : IEntityTypeConfiguration<SplitEntity>
	{
		public SplitEntityConfiguration()
		{
		}

        public void Configure(EntityTypeBuilder<SplitEntity> builder)
        {
            builder.ToTable("splits");

            builder.HasKey(x => x.SplitId);
            builder.Property(x => x.SplitId).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.CatCode).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.TransactionId).IsRequired();
        }
    }
}

