using System;
using System.Text.Json.Serialization;
using PFMBackendAPI.Models.Responses.dto;

namespace PFMBackendAPI.Models
{
	public class Split
	{

		public int SplitId { get; set; }
		public string Description { get; set; }
		public string CatCode { get; set; }
		public double Amount { get; set; }
		public int TransactionId { get; set; }
        [JsonIgnore]
        public Transaction Transaction { get; set;}

		public Split()
		{
		}

        public Split(SplitDto splitDto, int transactionId)
        {
			this.Description = splitDto.Description;
			this.CatCode = splitDto.catcode;
			this.Amount = splitDto.amount;
			this.TransactionId = transactionId;
        }
    }
}

