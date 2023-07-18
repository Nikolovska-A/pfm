using System;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Services
{
	public interface ITransactionService
	{

		Task<bool> ImportTransactions(List<Transaction> transactions);

        public bool TransactionExists(Transaction transaction);

    }
}

