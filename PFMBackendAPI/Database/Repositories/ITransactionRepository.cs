using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Repositories
{
	public interface ITransactionRepository
	{

        Task<bool> ImportTransactions(List<TransactionEntity> transactions);

        bool TransactionExists(TransactionEntity transaction);

        bool TransactionExistById(int transactionId);

        TransactionEntity GetTransactionById(int transactionId);

        Task<bool> UpdateTransaction(int transactionId, string catcode);

        Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 5, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);


    }
}

