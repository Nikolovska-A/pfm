using System;
namespace PFMBackendAPI.Models.Responses
{
	public class ErrorResponse
	{
		public string StatusCode { get; set; }
		public List<ErrorResponseDto> errors { get; set; }

		public ErrorResponse()
		{
		}
	}
}

