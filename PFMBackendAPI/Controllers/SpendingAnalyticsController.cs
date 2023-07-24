using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PFMBackendAPI.Models.Responses;
using PFMBackendAPI.Services;

namespace PFMBackendAPI.Controllers
{

    [ApiController]
    [Route("v1/spending-analytics")]
    public class SpendingAnalyticsController : ControllerBase
    {

        private readonly ILogger<SpendingAnalyticsController> _logger;
        private readonly ITransactionService _transactionService;

        public SpendingAnalyticsController(ITransactionService transactionService, ILogger<SpendingAnalyticsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }


        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> GetSpendingAnalytics([FromQuery] string? catCode, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] char direction)
        {
            if (startDate > endDate)
            {
                return BadRequest(new MessageResponse("The start date should be on or before the end date. Please adjust your dates accordingly."));
            }
            var result = await _transactionService.GetSpendingAnalytics(catCode, startDate, endDate, direction);
            SpendingAnalyticsResponse response = new SpendingAnalyticsResponse(result);
            if (response.groups.Count == 0)
            {
                return NotFound(new MessageResponse("No transactions were found!"));
            }
            return Ok(response);
        }
    }
}

