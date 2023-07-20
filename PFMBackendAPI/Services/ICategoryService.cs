using System;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Services
{
	public interface ICategoryService
	{

        Task<bool> ImportCategories(List<Category> categories, List<Category> updateCategories);

        public bool CategoryExists(Category category);

        public bool CategoryExistByParentCodeAndName(Category category);
    }
}

