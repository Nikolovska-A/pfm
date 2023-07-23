using System;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;
using PFMBackendAPI.Models.Responses;

namespace PFMBackendAPI.Services
{
    public interface ITransactionService
    {

        Task<bool> ImportTransactions(List<Transaction> transactions);

        bool TransactionExists(Transaction transaction);

        bool TransactionExistById(int transactionId);

        Transaction GetTransactionById(int transactionId);

        Task<bool> UpdateTransaction(int transactionId, string catcode);

        Task<PagedSortedList<Transaction>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, int page = 1,
       int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);

        Task<List<GroupAnalyticsDto>> GetSpendingAnalytics(string catCode, DateTime? startDate, DateTime? endDate, char direction);

        Task<bool> AutoCategorizeTransaction(string catcode, string predicate);

    }
}

