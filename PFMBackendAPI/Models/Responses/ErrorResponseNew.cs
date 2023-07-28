using System;
using PFMBackendAPI.Models.dto;

namespace PFMBackendAPI.Models.Responses
{
	public class ErrorResponseNew
	{
		public string statusCode { get; set; }
		public string message { get; set; }
		public List<ErrorResponseDtoWithRow> errors { get; set; }

		public ErrorResponseNew()
		{
			this.message = "One or more request errors occurred.";
		}
	}
}

