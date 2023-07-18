using System;
using Microsoft.EntityFrameworkCore;
using PFMBackendAPI.Database.Configurations;
using PFMBackendAPI.Database.Entities;

namespace PFMBackendAPI.Database
{
	public class TransactionDbContext : DbContext
	{
		public DbSet<TransactionEntity> Transactions { get; set; }


        public TransactionDbContext(DbContextOptions options) : base(options)
        {
        }

        public TransactionDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            modelBuilder.ApplyConfiguration(
                new TransactionEntityConfiguration()
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}

