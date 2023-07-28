using System;
namespace PFMBackendAPI.Models.Responses
{
	public class ImportFileMessageResponse
	{

		public string Message { get; set; }
		public int UpdatedRows { get; set; }
		public int InsertedRows { get; set; }

		public ImportFileMessageResponse()
		{
		}

        public ImportFileMessageResponse(string message, int updated, int inserted)
        {
			this.Message = message;
			this.UpdatedRows = updated;
			this.InsertedRows = inserted;
        }
    }
}

