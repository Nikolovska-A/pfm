using System;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Services
{
	public interface ICategoryService
	{

        Task<bool> ImportCategories(List<Category> categories, List<Category> updateCategories);

        bool CategoryExists(Category category);

        bool CategoryExistById(string categoryCode);

        bool CategoryExistByParentCodeAndName(Category category);

        Category GetCategoryByCode(string categoryCode);
    }
}

