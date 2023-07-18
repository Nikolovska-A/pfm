using System;
namespace PFMBackendAPI.Models.Responses
{
	public class MessageResponse
	{

		public string Message { get; set; }

		public MessageResponse()
		{
			this.Message = "OK";
		}

        public MessageResponse(string message)
        {
			this.Message = message;
        }
    }
}

