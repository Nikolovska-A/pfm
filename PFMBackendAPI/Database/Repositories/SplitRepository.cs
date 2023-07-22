using System;
using Microsoft.EntityFrameworkCore;
using PFMBackendAPI.Database.Entities;

namespace PFMBackendAPI.Database.Repositories
{
	public class SplitRepository : ISplitRepository
	{
        FinanceDbContext _dbContext;

        public SplitRepository(FinanceDbContext dbContext)
		{
            _dbContext = dbContext;
        }

        public bool SplitExistByTransactionId(int transactionId)
        {
            return _dbContext.Splits.Any(s => s.TransactionId == transactionId);
        }


        public async Task<List<SplitEntity>> CreateSplits(List<SplitEntity> splits)
        {
            _dbContext.Splits.AddRange(splits);
            await _dbContext.SaveChangesAsync();
            return splits;
        }


        public async Task<bool> DeleteSplits(int transactionId)
        {
            var splits = _dbContext.Splits.Where(s => s.TransactionId == transactionId);
            _dbContext.Splits.RemoveRange(splits);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

