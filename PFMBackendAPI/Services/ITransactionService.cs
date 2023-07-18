using System;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Services
{
	public interface ITransactionService
	{

		Task<bool> ImportTransactions(List<Transaction> transactions);

        public bool TransactionExists(Transaction transaction);

        public Task<PagedSortedList<Transaction>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, int page = 1,
           int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);

    }
}

