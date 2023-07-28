using System;
namespace PFMBackendAPI.Models.Responses
{
	public class BulkErrorResponse
	{

        public string statusCode { get; set; }
        public string message { get; set; }
       
        public BulkErrorResponse()
		{

        }
        public BulkErrorResponse(string code)
        {
            this.statusCode = code;
            this.message = "Due to a high number of validation errors, the import process was unsuccessful.";

        }
    }
}

