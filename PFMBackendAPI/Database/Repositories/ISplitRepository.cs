using System;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Repositories
{
	public interface ISplitRepository
	{
        bool SplitExistByTransactionId(int transactionId);

        Task<List<SplitEntity>> CreateSplits(List<SplitEntity> splits);

        Task<bool> DeleteSplits(int transactionId);
        
    }
}

