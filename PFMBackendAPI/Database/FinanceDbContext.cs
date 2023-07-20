using System;
using Microsoft.EntityFrameworkCore;
using PFMBackendAPI.Database.Configurations;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database
{
	public class FinanceDbContext : DbContext
	{
		public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }


        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {
        }

        public FinanceDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            modelBuilder.ApplyConfiguration(
                new TransactionEntityConfiguration()
                );
            modelBuilder.ApplyConfiguration(
               new CategoryEntityConfiguration()
               );
            modelBuilder.Entity<TransactionEntity>().HasOne(t => t.Category).WithMany(c => c.Transactions).HasForeignKey(t => t.CatCode).IsRequired(false);


            base.OnModelCreating(modelBuilder);

        }
    }
}
