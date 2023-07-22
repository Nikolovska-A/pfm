using System;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Services
{
	public interface ISplitService
	{
        bool SplitExistByTransactionId(int transactionId);

        Task<List<Split>> CreateSplits(List<Split> splits);

        Task<bool> DeleteSplits(int transactionId);
    }
}

