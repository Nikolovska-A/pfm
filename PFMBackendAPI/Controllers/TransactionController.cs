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
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.NetworkInformation;
using PFMBackendAPI.Models.dto;

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
    private readonly GetRules _getRules;

    public TransactionController(ITransactionService transactionService, ICategoryService categoryService, ISplitService splitService, ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _categoryService = categoryService;
        _splitService = splitService;
        _logger = logger;
        _csvFileReader = new CsvFileReader();
        _getRules = new GetRules();
    }


    [Produces("application/json")]
    [Route("import")]
    [HttpPost]
    public async Task<IActionResult> ImportTransactionsAsync([FromForm] IFormFile formFile)
    {
        List<Transaction> transactions = new List<Transaction>();
        List<Transaction> updateTransactions = new List<Transaction>();
        List<TransactionCsvLine> csvTransactions = new List<TransactionCsvLine>();
        List<ErrorResponseDtoWithRow> errorList = new List<ErrorResponseDtoWithRow>();
        ErrorResponseNew errors = new ErrorResponseNew();
        int row = 1;

        try
        {
            List<Transaction> listTransactions = await _transactionService.GetAllTransactions();
            Dictionary<int, Transaction> transactionsMap = new Dictionary<int, Transaction>();

            foreach (Transaction t in listTransactions)
            {
                transactionsMap.Add(t.TransactionId, t);
            }

            if (!await _csvFileReader.GetCsvReader(formFile, 1)) { throw new Exception("Something went wrong!"); }

            csvTransactions = _csvFileReader.transactionCsvLines;

            foreach (TransactionCsvLine line in csvTransactions)
            {
                if (line.id != null)
                {
                    Transaction tempTransaction = new Transaction(line);

                    if (!_transactionService.TransactionExists(tempTransaction))
                    {
                        errorList.AddRange(_transactionService.GetValidations(tempTransaction, row));

                        transactions.Add(tempTransaction);
                    }
                    else
                    {
                        if (!transactionsMap[tempTransaction.TransactionId].Equals(tempTransaction))
                        {
                            errorList = _transactionService.GetValidations(tempTransaction, row);

                            updateTransactions.Add(tempTransaction);
                        }
                    }
                    row++;
                }
            }

            if (errorList.Count == 0)
            {
                var result = await _transactionService.ImportTransactions(transactions, updateTransactions);
                var transactionsImported = transactions.Count;
                var transactionsUpdated = updateTransactions.Count;

                return Ok(new ImportFileMessageResponse("Transactions updated/imported successfully!", transactionsUpdated, transactionsImported));

            }
            else if (errorList.Count >= 100)
            {
                BulkErrorResponse error = new BulkErrorResponse(errors.statusCode = BadRequest().StatusCode.ToString());
                return BadRequest(error);
            }

            else
            {
                errors.statusCode = BadRequest().StatusCode.ToString();
                errors.errors = errorList;
                return BadRequest(errors);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);

        }
    }


    [HttpGet]
    public async Task<IActionResult> GetTransactions([FromQuery] string? transactionKind, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate,
        [FromQuery] int? page, [FromQuery] int? pageSize, [FromQuery] string? sortBy, [FromQuery] SortOrder sortOrder)
    {
        page = page ?? 1;
        pageSize = pageSize ?? 10;

        if (startDate > endDate)
        {
            return BadRequest(new MessageResponse("The start date should be on or before the end date. Please adjust your dates accordingly."));
        }

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

                    return Ok(new MessageResponse("Transaction updated successfully!"));
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

                        if (split.amount <= 0.0)
                        {
                            errorsList.Add(new ErrorResponseDto("amount", "invalid-input", "The amount cannot be less than 0.0"));
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
                errors.statusCode = NotFound().StatusCode.ToString();
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
                errors.statusCode = BadRequest().StatusCode.ToString();
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
            List<Rule> rules = _getRules.GetRulesList(resourceName);

            foreach (Rule rule in rules)
            {
                var result = await _transactionService.AutoCategorizeTransaction(rule.catcode, rule.predicate);
            }
            return Ok(new MessageResponse("Transactions were auto categorized successfully!"));
        }
        catch (Exception e)
        {
            return BadRequest(new MessageResponse(e.Message));
        }

    }


    [Produces("application/json")]
    [Route("auto-categorize-new")]
    [HttpPost]
    public async Task<IActionResult> AutoCategorizeTransactionNew()
    {
        try
        {
            string resourceName = "PFMBackendAPI.resources.rules-new.yaml";
            List<Rule> rules = _getRules.GetRulesList(resourceName);

            foreach (Rule rule in rules)
            {
                var result = await _transactionService.AutoCategorizeTransactionNew(rule.catcode, rule.predicate);
            }
            return Ok(new MessageResponse("Transactions were auto categorized successfully!"));
        }
        catch (Exception e)
        {
            return BadRequest(new MessageResponse(e.Message));
        }

    }


}