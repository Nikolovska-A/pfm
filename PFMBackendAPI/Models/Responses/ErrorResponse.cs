using System;
using System.Text.Json;

namespace PFMBackendAPI.Models.Responses
{

	[Serializable]
	public class ErrorResponse
	{
		public string Tag { get; set; }
		public string Error { get; set; }
		public string Message { get; set; }

		public ErrorResponse()
		{
		}

		public ErrorResponse(string tag, string error, string message)
		{
			this.Tag = tag;
			this.Error = error;
			this.Message = message;
		}

	}
}

