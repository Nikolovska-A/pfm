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

        Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 5, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);


    }
}

