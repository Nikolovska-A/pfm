using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Database.Repositories;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Services
{
	public class CategoryService : ICategoryService
    {
        ICategoryRepository _categoryRepository;
        IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _categoryRepository = repository;
            _mapper = mapper;
        }

        public async Task<bool> ImportCategories(List<Category> categories, List<Category> updateCategories)
        {
            var entity = _mapper.Map<List<CategoryEntity>>(categories);
            var updateEntity = _mapper.Map<List<CategoryEntity>>(updateCategories);

            var response = await _categoryRepository.ImportCategories(entity, updateEntity);
            return response;
        }

        public bool CategoryExists(Category category)
        {
            var entity = _mapper.Map<CategoryEntity>(category);
            return _categoryRepository.CategoryExists(entity);
        }

        public bool CategoryExistById(string categoryCode)
        {
            return _categoryRepository.CategoryExistById(categoryCode);
        }

        public bool CategoryExistByParentCodeAndName(Category category)
        {
            var entity = _mapper.Map<CategoryEntity>(category);
            return _categoryRepository.CategoryExistByParentCodeAndName(entity);
        }

        public Category GetCategoryByCode(string categoryCode)
        {
            var entity = _mapper.Map<Category>(_categoryRepository.GetCategoryByCode(categoryCode));
            return entity;
        }
    }
}

