using System;
namespace PFMBackendAPI.Models.Responses
{
	public class Error
	{
		public string StatusCode { get; set; }
		public List<ErrorResponse> errors { get; set; }

		public Error()
		{
		}
	}
}

