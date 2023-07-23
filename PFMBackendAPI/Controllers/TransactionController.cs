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
using PFMBackendAPI.Models.Responses.dto;
using System.Reflection;
using System.Numerics;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PFMBackendAPI.Controllers;

[ApiController]
[Route("v1/transactions")]
public class TransactionController : ControllerBase
{

    private readonly ILogger<TransactionController> _logger;
    private readonly ITransactionService _transactionService;
    private readonly CsvFileReader _csvFileReader;
    private readonly ICategoryService _categoryService;
    private readonly ISplitService _splitService;

    public TransactionController(ITransactionService transactionService, ICategoryService categoryService, ISplitService splitService, ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _categoryService = categoryService;
        _splitService = splitService;
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
        List<ErrorResponseDto> errorList = new List<ErrorResponseDto>();
        ErrorResponse errors = new ErrorResponse();

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
                            errorList.Add(new ErrorResponseDto("amount", "required", "Mandatory field or parameter was not supplied."));
                        }

                        if (tempTransaction.Direction.Equals('\0'))
                        {
                            errorList.Add(new ErrorResponseDto("direction", "required", "Mandatory field or parameter was not supplied."));
                        }

                        if (!(tempTransaction.Direction.Equals('c') || tempTransaction.Direction.Equals('d')))
                        {
                            errorList.Add(new ErrorResponseDto("direction", "invalid-format", "Value supplied does not have expected format."));
                        }

                        transactions.Add(tempTransaction);
                    }

                }
            }

            if (errorList.Count == 0)
            {
                var result = await _transactionService.ImportTransactions(transactions);
                return Ok(new MessageResponse("Transactions imported successfully!"));
            }
            else
            {
                errors.StatusCode = BadRequest().StatusCode.ToString();
                errors.errors = errorList;
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

            if (transaction != null)
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
        }
        catch (Exception e)
        {
            return BadRequest(new MessageResponse(e.Message));
        }
    }


    [Produces("application/json")]
    [Route("{id}/split")]
    [HttpPost]
    public async Task<IActionResult> SplitTransaction([FromRoute] int id, [FromBody] SplitRequest splitRequest)
    {
        try
        {
            List<Split> splits = new List<Split>();
            List<ErrorResponseDto> errorsList = new List<ErrorResponseDto>();
            ErrorResponse errors = new ErrorResponse();
            Transaction transaction = new Transaction();

            if (_transactionService.TransactionExistById(id))
            {
                double sumAmount = 0.0;
                transaction = _transactionService.GetTransactionById(id);

                if (splitRequest.splits.Count > 1)
                {
                    foreach (SplitDto split in splitRequest.splits)
                    {
                        sumAmount += split.amount;

                        if (!_categoryService.CategoryExistById(split.catcode))
                        {
                            errorsList.Add(new ErrorResponseDto("catcode", "invalid-input", string.Format("The provided category code: '{0}' does not exist!", split.catcode)));
                        }

                        splits.Add(new Split(split, transaction.TransactionId));
                    }

                    if (transaction.Amount != sumAmount || sumAmount == 0.0)
                    {
                        errorsList.Add(new ErrorResponseDto("amount", "invalid-split", "The sum of the split amounts is not equal to the transaction amount."));
                    }
                }
                else
                {
                    errorsList.Add(new ErrorResponseDto("splits", "invalid-format", "The transaction cannot be split into one or less then one splits."));
                }
            }
            else
            {
                errorsList.Add(new ErrorResponseDto("transaction", "not-found", string.Format("Transaction with the provided transaction id: '{0}' does not exist!", id)));
                errors.errors = errorsList;
                errors.StatusCode = NotFound().StatusCode.ToString();
                return NotFound(errors);
            }
            if (errorsList.Count == 0)
            {
                if (_splitService.SplitExistByTransactionId(transaction.TransactionId))
                {
                    await _splitService.DeleteSplits(transaction.TransactionId);
                }
                await _splitService.CreateSplits(splits);
            }
            else
            {
                errors.errors = errorsList;
                errors.StatusCode = BadRequest().StatusCode.ToString();
                return BadRequest(errors);
            }
            return Ok(new MessageResponse("Transaction splits saved successfully!"));
        }
        catch (Exception e)
        {
            return BadRequest(new MessageResponse(e.Message));
        }
    }


    [Produces("application/json")]
    [Route("auto-categorize")]
    [HttpPost]
    public async Task<IActionResult> AutoCategorizeTransaction()
    {
        try
        {
            string resourceName = "PFMBackendAPI.resources.rules.yaml";
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return NotFound(new MessageResponse("File not found!"));
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(CamelCaseNamingConvention.Instance)
                        .Build();
                    List<Rule> rules = deserializer.Deserialize<List<Rule>>(content);

                    foreach (Rule rule in rules)
                    {
                        var result = await _transactionService.AutoCategorizeTransaction(rule.catcode, rule.predicate);
                    }
                    return Ok(new MessageResponse("Transactions were auto categorized succeffully!"));
                }
            }
        }
        catch (Exception e)
        {
            return BadRequest(new MessageResponse(e.Message));
        }

    }


}