using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Database.Repositories;
using PFMBackendAPI.Models;
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

        public async Task<bool> ImportTransactions(List<Transaction> transactions)
        {
            var entity = _mapper.Map<List<TransactionEntity>>(transactions);
            var response = await _transactionRepository.ImportTransactions(entity);
            return response;
        }

        public bool TransactionExists(Transaction transaction)
        {
            var entity = _mapper.Map<TransactionEntity>(transaction);
            return _transactionRepository.TransactionExists(entity);
        }
    }
}

