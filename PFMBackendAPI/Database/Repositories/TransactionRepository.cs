using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Repositories
{
	public class TransactionRepository : ITransactionRepository
    {
        TransactionDbContext _dbContext;

		public TransactionRepository(TransactionDbContext dbContext)
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
            return _dbContext.Transactions.Any(t => t.TransactionId != transaction.TransactionId);
            
        }

        public string BeneficiaryName { get; set; }
        public DateTime Date { get; set; }
        public char Direction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public Currency Currency { get; set; }
        public int Mcc { get; set; }
        public string Kind { get; set; }


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



    }
}

