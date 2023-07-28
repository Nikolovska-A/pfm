using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PFMBackendAPI.Models.Responses
{

	[Serializable]
	public class ErrorResponseDto
	{
		public string Tag { get; set; }
		public string Error { get; set; }
		public string Message { get; set; }

		public ErrorResponseDto()
		{
		}

		public ErrorResponseDto(string tag, string error, string message)
		{
			this.Tag = tag;
			this.Error = error;
			this.Message = message;
		}

    }
}

