using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PFMBackendAPI.Database.Entities;

namespace PFMBackendAPI.Database.Configurations
{
	public class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public CategoryEntityConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(x => x.Code);
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.ParentCode);
            builder.Property(x => x.Name);

        }
    }
}

