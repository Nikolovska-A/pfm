using System;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Repositories
{
	public class TransactionRepository : ITransactionRepository
    {
        TransactionDbContext _dbContext;

		public TransactionRepository(TransactionDbContext dbContext)
		{
            _dbContext = dbContext;
		}

        public async Task<bool> ImportTransactions(List<TransactionEntity> transactions)
        {
            _dbContext.Transactions.AddRange(transactions);

            await _dbContext.SaveChangesAsync();

            return true;

        }

        public bool TransactionExists(TransactionEntity transaction)
        {
            return _dbContext.Transactions.Any(t => t.TransactionId != transaction.TransactionId);
            
        }
    }
}

