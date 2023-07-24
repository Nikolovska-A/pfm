using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Database.Repositories;
using PFMBackendAPI.Models;
using PFMBackendAPI.Models.Responses;
using SortOrder = PFMBackendAPI.Models.SortOrder;
using Transaction = PFMBackendAPI.Models.Transaction;

namespace PFMBackendAPI.Services
{
	public class TransactionService : ITransactionService
	{
        ITransactionRepository _transactionRepository;
        IMapper _mapper;

		public TransactionService(ITransactionRepository repository, IMapper mapper)
		{
            _transactionRepository = repository;
            _mapper = mapper;
		}

        public async Task<bool> ImportTransactions(List<Transaction> transactions, List<Transaction> updateTransactions)
        {
            var entity = _mapper.Map<List<TransactionEntity>>(transactions);
            var updateEntity = _mapper.Map<List<TransactionEntity>>(updateTransactions);
            var response = await _transactionRepository.ImportTransactions(entity, updateEntity);
            return response;
        }

        public bool TransactionExists(Transaction transaction)
        {
            var entity = _mapper.Map<TransactionEntity>(transaction);
            return _transactionRepository.TransactionExists(entity);
        }

        public bool TransactionExistById(int transactionId)
        {
            return _transactionRepository.TransactionExistById(transactionId);
        }

        public Transaction GetTransactionById(int transactionId)
        {
            var entity = _mapper.Map<Transaction>(_transactionRepository.GetTransactionById(transactionId));
            return entity;
        }

        public async Task<bool> UpdateTransaction(int transactionId, string catcode)
        {
            var entity = _mapper.Map<TransactionEntity>(_transactionRepository.GetTransactionById(transactionId));
            return await _transactionRepository.UpdateTransaction(entity.TransactionId, catcode);
        }


        public async Task<PagedSortedList<Transaction>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, int page = 1,
            int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            var result = await _transactionRepository.GetTransactions(transactionKind, startDate, endDate, page, pageSize, sortBy, sortOrder);

            return _mapper.Map<PagedSortedList<Transaction>>(result);
        }


        public async Task<List<GroupAnalyticsDto>> GetSpendingAnalytics(string catCode, DateTime? startDate, DateTime? endDate, char direction)
        {
            var result = await _transactionRepository.GetSpendingAnalytics(catCode, startDate, endDate, direction);
            return result;
        }

        public async Task<bool> AutoCategorizeTransaction(string catcode, string predicate)
        {
            var result = await _transactionRepository.AutoCategorizeTransaction(catcode, predicate);
            return result;
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            var result = await _transactionRepository.GetAllTransactions();
            return _mapper.Map<List<Transaction>>(result);
        }
    }

    
}

