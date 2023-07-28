using System;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;
using PFMBackendAPI.Models.dto;
using PFMBackendAPI.Models.Responses;

namespace PFMBackendAPI.Services
{
    public interface ITransactionService
    {

        Task<bool> ImportTransactions(List<Transaction> transactions, List<Transaction> updateTransactions);

        bool TransactionExists(Transaction transaction);

        bool TransactionExistById(int transactionId);

        Transaction GetTransactionById(int transactionId);

        Task<bool> UpdateTransaction(int transactionId, string catcode);

        Task<PagedSortedList<Transaction>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, int page = 1,
       int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.asc);

        Task<List<GroupAnalyticsDto>> GetSpendingAnalytics(string catCode, DateTime? startDate, DateTime? endDate, char direction);

        Task<bool> AutoCategorizeTransaction(string catcode, string predicate);

        Task<List<Transaction>> GetAllTransactions();

        Task<bool> AutoCategorizeTransactionNew(string catcode, string predicate);

        public List<ErrorResponseDtoWithRow> GetValidations(Transaction transaction, int row);

    }
}

