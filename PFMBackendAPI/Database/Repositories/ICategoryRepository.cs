using System;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Repositories
{
	public interface ICategoryRepository
	{

        Task<bool> ImportCategories(List<CategoryEntity> categories, List<CategoryEntity> updateCategories);

        bool CategoryExists(CategoryEntity category);

        bool CategoryExistById(string categoryCode);

        bool CategoryExistByParentCodeAndName(CategoryEntity category);

        CategoryEntity GetCategoryByCode(string categoryCode);

        Task<PagedSortedList<CategoryEntity>> GetCategories(string parentCode, int page = 1, int pageSize = 5, string sortBy = null, SortOrder sortOrder = SortOrder.Asc);

        Task<List<CategoryEntity>> GetAllCategories();
    }
}

