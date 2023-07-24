using System;
using Microsoft.AspNetCore.Mvc;

namespace PFMBackendAPI.Helpers
{
    public class ErrorResponseValidator
    {
        public ErrorResponseValidator()
        {
        }

        public static IActionResult MakeValidationResponse(ActionContext context)
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
            };
            var errorResponseDetails = new ErrorResponseDetails
            {
                statusCode = problemDetails.Status,
                message = problemDetails.Title,
                errors = problemDetails.Errors,
            };

            var result = new BadRequestObjectResult(errorResponseDetails);
            result.ContentTypes.Add("application/problem+json");
            return result;
        }

    }

}

