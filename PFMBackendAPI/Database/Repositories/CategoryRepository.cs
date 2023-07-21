using System;
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
    }
}

