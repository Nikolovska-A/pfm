using System;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Helpers;
using PFMBackendAPI.Models;
using PFMBackendAPI.Models.Responses;
using PFMBackendAPI.Services;

namespace PFMBackendAPI.Controllers
{

    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly CsvFileReader _csvFileReader;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
            _csvFileReader = new CsvFileReader();
        }


        [Produces("application/json")]
        [Route("import")]
        [HttpPost]
        public async Task<IActionResult> ImportCategoriesAsync([FromForm] IFormFile formFile)
        {

            if (formFile.FileName == null)
            {
                return BadRequest("File does not exist!");
            }

            List<Category> categories = new List<Category>();
            List<Category> updateCategories = new List<Category>();
            List<CategoryCsvLine> csvCategories = new List<CategoryCsvLine>();
            List<ErrorResponse> errors = new List<ErrorResponse>();

            try
            {
                if (!await _csvFileReader.GetCsvReader(formFile, 2)) { throw new Exception("Something went wrong!"); }
                csvCategories = _csvFileReader.categoryCsvLines;

                foreach (CategoryCsvLine line in csvCategories)
                {
                    if (line.code != null)
                    {
                        Category tempCategory = new Category().toCategory(line);
                        if (!_categoryService.CategoryExists(tempCategory))
                        {
                            if (!_categoryService.CategoryExistByParentCodeAndName(tempCategory))
                            {
                                categories.Add(tempCategory);
                            }

                            else
                            {
                                errors.Add(new ErrorResponse("parentCode, name", "Duplicate entries", string.Format("This combination of parent code: '{0}' and name: '{1}' already exists for another entry. " +
                                    "Please provide a different parent code and name or update the existing entry accordingly.", tempCategory.ParentCode, tempCategory.Name)));
                            }

                        }

                        else
                        {
                            updateCategories.Add(tempCategory);
                        }
                    }

                }


                if (errors.Count == 0)
                {
                    var result = await _categoryService.ImportCategories(categories, updateCategories);
                    return Ok(new MessageResponse("Categories imported and/or updated successfully!"));
                }
                else
                {
                    return BadRequest(errors);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse(ex.Message));
            }
        }
    }
}
