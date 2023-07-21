﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;
using PFMBackendAPI.Models.Responses;

namespace PFMBackendAPI.Database.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        FinanceDbContext _dbContext;

        public TransactionRepository(FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ImportTransactions(List<TransactionEntity> transactions)
        {
            _dbContext.Transactions.AddRange(transactions);

            await _dbContext.SaveChangesAsync();

            return true;

        }

        public bool TransactionExists(TransactionEntity transaction)
        {
            return _dbContext.Transactions.Any(t => t.TransactionId == transaction.TransactionId);

        }

        public bool TransactionExistById(int transactionId)
        {
            return _dbContext.Transactions.AsNoTracking().Any(t => t.TransactionId == transactionId);

        }

        public TransactionEntity GetTransactionById(int transactionId)
        {
            return _dbContext.Transactions.Find(transactionId);
        }

        public async Task<bool> UpdateTransaction(int transactionId, string catcode)
        {
            var transactionToUpdate = _dbContext.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);

            if (transactionToUpdate == null)
            {
                throw new Exception("Transaction not found!");
            }

            transactionToUpdate.CatCode = catcode;
            await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<PagedSortedList<TransactionEntity>> GetTransactions(string transactionKind, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            var query = _dbContext.Transactions.AsQueryable();

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "transactionId":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.TransactionId) : query.OrderByDescending(x => x.TransactionId);
                        break;
                    case "date":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
                        break;
                    case "direction":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Direction) : query.OrderByDescending(x => x.Direction);
                        break;
                    case "amount":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Amount) : query.OrderByDescending(x => x.Amount);
                        break;
                    case "description":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Description) : query.OrderByDescending(x => x.Description);
                        break;
                    case "currency":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Currency) : query.OrderByDescending(x => x.Currency);
                        break;
                    case "mcc":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Mcc) : query.OrderByDescending(x => x.Mcc);
                        break;
                    case "kind":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.Kind) : query.OrderByDescending(x => x.Kind);
                        break;
                    default:
                    case "beneficiaryName":
                        query = sortOrder == SortOrder.Asc ? query.OrderBy(x => x.BeneficiaryName) : query.OrderByDescending(x => x.BeneficiaryName);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(p => p.BeneficiaryName);
            }


            if (!String.IsNullOrEmpty(transactionKind))
            {
                var values = transactionKind.Split(',');
                query = query.Where(t => values.Contains(t.Kind));
            }

            if (startDate != null)
            {
                query = query.Where(t => t.Date >= startDate);
            }

            if (endDate != null)
            {
                query = query.Where(t => t.Date <= endDate);
            }

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var items = await query.ToListAsync();

            return new PagedSortedList<TransactionEntity>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Items = items,
                SortBy = sortBy,
                SortOrder = sortOrder
            };
        }



        public async Task<List<GroupAnalyticsDto>> GetSpendingAnalytics(string catCode, DateTime? startDate, DateTime? endDate, char direction)
        {
            var query = _dbContext.Transactions.AsQueryable();


            if (!String.IsNullOrEmpty(catCode))
            {
                var values = catCode.Split(',');
                query = query.Where(t => t.CatCode == catCode);
            }

            if (startDate != null)
            {
                query = query.Where(t => t.Date >= startDate);
            }

            if (endDate != null)
            {
                query = query.Where(t => t.Date <= endDate);
            }

            if (direction != '\0')
            {
                query = query.Where(t => t.Direction == direction);
            }

            var tquery = query.GroupBy(f => f.CatCode)
            .Select(g => new GroupAnalyticsDto
            {
                catcode = g.Key,
                count = g.Count(),
                amount = g.Sum(s => s.Amount),

            });

            List<GroupAnalyticsDto> groupAnalyticsDtos = new List<GroupAnalyticsDto>();
            groupAnalyticsDtos = await tquery.ToListAsync();

            return groupAnalyticsDtos;
        }


    }
}

