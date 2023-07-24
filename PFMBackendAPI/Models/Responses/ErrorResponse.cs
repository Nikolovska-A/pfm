using System;
namespace PFMBackendAPI.Models.Responses
{
	public class ErrorResponse
	{
		public string statusCode { get; set; }
		public string message { get; set; }
		public List<ErrorResponseDto> errors { get; set; }

		public ErrorResponse()
		{
			this.message = "One or more request errors occurred.";
		}
	}
}

