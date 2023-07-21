using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using PFMBackendAPI.Helpers;
using PFMBackendAPI.Models;
using PFMBackendAPI.Models.Responses;
using PFMBackendAPI.Services;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SortOrder = PFMBackendAPI.Models.SortOrder;
using PFMBackendAPI.Models.Requests;

namespace PFMBackendAPI.Controllers;

[ApiController]
[Route("v1/transactions")]
public class TransactionController : ControllerBase
{

    private readonly ILogger<TransactionController> _logger;
    private readonly ITransactionService _transactionService;
    private readonly CsvFileReader _csvFileReader;
    private readonly ICategoryService _categoryService;

    public TransactionController(ITransactionService transactionService, ICategoryService categoryService, ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _categoryService = categoryService;
        _logger = logger;
        _csvFileReader = new CsvFileReader();
    }


    [Produces("application/json")]
    [Route("import")]
    [HttpPost]
    public async Task<IActionResult> ImportTransactionsAsync([FromForm] IFormFile formFile)
    {
        if (formFile.FileName == null)
        {
            return BadRequest(new MessageResponse("File does not exist!"));
        }

        List<Transaction> transactions = new List<Transaction>();
        List<TransactionCsvLine> csvTransactions = new List<TransactionCsvLine>();
        List<ErrorResponse> errors = new List<ErrorResponse>();

        try
        {

            if (!await _csvFileReader.GetCsvReader(formFile, 1)) { throw new Exception("Something went wrong!"); }

            csvTransactions = _csvFileReader.transactionCsvLines;

            foreach (TransactionCsvLine line in csvTransactions)
            {
                if (line.id != null)
                {
                    Transaction tempTransaction = new Transaction(line);

                    if (!_transactionService.TransactionExists(tempTransaction))
                    {

                        if (tempTransaction.Amount == 0)
                        {
                            errors.Add(new ErrorResponse("amount", "required", "Mandatory field or parameter was not supplied."));
                        }

                        if (tempTransaction.Direction.Equals('\0'))
                        {
                            errors.Add(new ErrorResponse("direction", "required", "Mandatory field or parameter was not supplied."));
                        }

                        if (!(tempTransaction.Direction.Equals('c') || tempTransaction.Direction.Equals('d')))
                        {
                            errors.Add(new ErrorResponse("direction", "invalid-format", "Value supplied does not have expected format."));
                        }

                        transactions.Add(tempTransaction);
                    }

                }
            }

            if (errors.Count == 0)
            {
                var result = await _transactionService.ImportTransactions(transactions);
                return Ok(new MessageResponse("Transactions imported successfully!"));
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
    public async Task<IActionResult> GetTransactions([FromQuery] string? transactionKind, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate,
        [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? sortBy, [FromQuery] SortOrder sortOrder)
    {
        page = page ?? 1;
        pageSize = pageSize ?? 10;

        var result = await _transactionService.GetTransactions(transactionKind, startDate, endDate, page.Value, pageSize.Value, sortBy, sortOrder);
        return Ok(result);
    }


    [Produces("application/json")]
    [Route("{id}/categorize")]
    [HttpPost]
    public async Task<IActionResult> CategorizeTransaction([FromRoute] int id, [FromBody] CategoryRequest categoryRequest)
    {
        try
        {
            Transaction transaction = _transactionService.GetTransactionById(id);

            if(transaction != null)
            {
                Category category = _categoryService.GetCategoryByCode(categoryRequest.catcode);

                if (category != null)
                {

                   await _transactionService.UpdateTransaction(transaction.TransactionId, categoryRequest.catcode);

                    return Ok(new MessageResponse("Transaction updated succeffully!"));
                }
                else
                {
                    return NotFound(new MessageResponse("Category not found!"));
                }

            }
            else
            {
                return NotFound(new MessageResponse("Transaction not found!"));
            }
        } catch (Exception e)
        {
            return BadRequest(new MessageResponse(e.Message));
        }
    }


}

