using System;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Repositories
{
	public interface ITransactionRepository
	{

        Task<bool> ImportTransactions(List<TransactionEntity> transactions);

        bool TransactionExists(TransactionEntity transaction);

    }
}

