using System;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Helpers;
using PFMBackendAPI.Models;
using PFMBackendAPI.Models.dto;
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

        private const int CategoryCsvFileType = 2;

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
                return BadRequest(new MessageResponse("File does not exist!"));
            }

            List<Category> categories = new List<Category>();
            List<Category> updateCategories = new List<Category>();
            List<CategoryCsvLine> csvCategories = new List<CategoryCsvLine>();
            List<ErrorResponseDtoWithRow> errors = new List<ErrorResponseDtoWithRow>();
            int row = 2;

            try
            {
                List<Category> listCategories = await _categoryService.GetAllCategories();
                Dictionary<string, Category> categoriesMap = new Dictionary<string, Category>();

                foreach (Category c in listCategories)
                {
                    categoriesMap.Add(c.CodeId, c);
                }

                if (!await _csvFileReader.GetCsvReader(formFile, CategoryCsvFileType)) { throw new Exception("Something went wrong!"); }
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
                                errors.Add(new ErrorResponseDtoWithRow("parentCode, name", "Duplicate entries", string.Format("This combination of parent code: '{0}' and name: '{1}' already exists for another entry. " +
                                    "Please provide a different parent code and name or update the existing entry accordingly.", tempCategory.ParentCode, tempCategory.Name), row));
                            }

                            if (string.IsNullOrEmpty(tempCategory.CodeId) && string.IsNullOrEmpty(tempCategory.ParentCode))
                            {
                                errors.Add(new ErrorResponseDtoWithRow("codeId, parentCode", "empty", "The codeId and parentCode for the entry are both empty. Please provide at least one of the codes.", row));
                            }
                        }
                        else
                        {
                            if (!categoriesMap[tempCategory.CodeId].Equals(tempCategory))
                            { 
                                updateCategories.Add(tempCategory);
                            }
                        }
                        row++;
                    }

                }

                if (errors.Count == 0)
                {
                    var result = await _categoryService.ImportCategories(categories, updateCategories);
                    var categoriesImported = categories.Count;
                    var categoriesUpdated = updateCategories.Count;

                    return Ok(new ImportFileMessageResponse("Categories updated/imported successfully!", categoriesUpdated, categoriesImported));
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


        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] string? parentCode, [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? sortBy, [FromQuery] SortOrder sortOrder)
        {
            page = page ?? 1;
            pageSize = pageSize ?? 10;

            var result = await _categoryService.GetCategories(parentCode, page.Value, pageSize.Value, sortBy, sortOrder);

            return Ok(result);
        }
    }
}
