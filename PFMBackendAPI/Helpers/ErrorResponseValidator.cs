using System;
using Microsoft.AspNetCore.Mvc;

namespace PFMBackendAPI.Helpers
{
    public class ErrorResponseValidator
    {
        public ErrorResponseValidator()
        {
        }


        /// <summary>
        /// Creates a validation response containing error details/>.
        /// </summary>
        /// <param name="context">The action context containing errors.</param>
        /// <returns>An IActionResult representing the validation problem response.</returns>
        public static IActionResult MakeValidationResponse(ActionContext context)
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest,
            };
            List<IDictionary<string, string[]>> listErrors = new List<IDictionary<string, string[]>>();

            listErrors.Add(problemDetails.Errors);

            var errorResponseDetails = new ErrorResponseDetails
            {
                statusCode = problemDetails.Status,
                message = problemDetails.Title,
                errors = listErrors
            };

            var result = new BadRequestObjectResult(errorResponseDetails);
            result.ContentTypes.Add("application/problem+json");
            return result;
        }

    }

}

