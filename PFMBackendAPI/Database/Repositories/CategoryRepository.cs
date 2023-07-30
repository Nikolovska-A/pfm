using System;
using Microsoft.EntityFrameworkCore;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
        FinanceDbContext _dbContext;

        public CategoryRepository(FinanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ImportCategories(List<CategoryEntity> categories, List<CategoryEntity> updateCategories)
        {
            _dbContext.ChangeTracker.Clear();
            _dbContext.Categories.UpdateRange(updateCategories);
            _dbContext.Categories.AddRange(categories);

            await _dbContext.SaveChangesAsync();

            return true;
                    
        }

        public bool CategoryExists(CategoryEntity category)
        {
            return _dbContext.Categories.Any(c => c.Code == category.Code);

        }

        public bool CategoryExistById(string categoryCode)
        {
            return _dbContext.Categories.Any(t => t.Code == categoryCode);

        }

        public bool CategoryExistByParentCodeAndName(CategoryEntity category)
        {
            return _dbContext.Categories.Any(c => c.ParentCode.Equals(category.ParentCode) && c.Name.Equals(category.Name));
        }

        public CategoryEntity GetCategoryByCode(string categoryCode)
        {
            return _dbContext.Categories.Find(categoryCode);
        }


        public async Task<PagedSortedList<CategoryEntity>> GetCategories(string parentCode, int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.asc)
        {
            pageSize = Math.Abs(pageSize);
            page = Math.Abs(page);

            var query = _dbContext.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "parentCode":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.ParentCode) : query.OrderByDescending(x => x.ParentCode);
                        break;
                    default:
                    case "name":
                        query = sortOrder == SortOrder.asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(p => p.Name);
                sortBy = "name";
            }

            if (!String.IsNullOrEmpty(parentCode))
            {
                var values = parentCode.Split(',');
                query = query.Where(t => values.Contains(t.ParentCode));
            }

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount * 1.0 / pageSize);
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var items = await query.Include(c => c.Transactions).ToListAsync();

            return new PagedSortedList<CategoryEntity>
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

        public async Task<List<CategoryEntity>> GetAllCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }

    }
}

