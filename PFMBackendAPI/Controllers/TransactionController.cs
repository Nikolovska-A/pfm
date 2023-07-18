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

namespace PFMBackendAPI.Controllers;

[ApiController]
[Route("v1/transactions")]
public class TransactionController : ControllerBase
{

    private readonly ILogger<TransactionController> _logger;
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }


    [Produces("application/json")]
    [Route("import")]
    [HttpPost]
    public async Task<IActionResult> ImportTransactionsAsync([FromForm] IFormFile formFile)
    {
        if (formFile.FileName == null)
        {
            return BadRequest();
        }

        List<Transaction> transactions = new List<Transaction>();
        List<CsvLine> csvTransactions = new List<CsvLine>();
        List<ErrorResponse> errors = new List<ErrorResponse>();

        using (var stream = new MemoryStream())
        {
            await formFile.CopyToAsync(stream);
            byte[] bytes = stream.ToArray();

            try
            {
                using (var fileStream = new FileStream(formFile.FileName, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(bytes, 0, bytes.Length);
                    using (var reader = new StreamReader(fileStream.Name, System.Text.Encoding.UTF8))
                    {
                        var config = new CsvConfiguration(System.Globalization.CultureInfo.CreateSpecificCulture("enUS"))
                        {
                            Delimiter = ",",
                            HasHeaderRecord = true,
                            TrimOptions = TrimOptions.Trim,
                            MissingFieldFound = null,
                            PrepareHeaderForMatch = args => Regex.Replace(args.Header, "-", "").ToLower()
                        };
                        var csv = new CsvReader(reader, config);
                        csvTransactions = csv.GetRecords<CsvLine>().ToList();

                        foreach (CsvLine line in csvTransactions)
                        {
                            if (line.id != null)
                            {
                                Transaction tempTransaction = new Transaction(line);

                                if (!_transactionService.TransactionExists(tempTransaction))
                                {

                                    if (tempTransaction.Amount == 0)
                                    {
                                        errors.Add(new ErrorResponse("amount", "Required", "Mandatory field or parameter was not supplied."));
                                    }

                                    if (tempTransaction.Direction.Equals('\0'))
                                    {
                                        errors.Add(new ErrorResponse("direction", "Required", "Mandatory field or parameter was not supplied."));
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
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new MessageResponse(ex.Message));
            }
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


}

