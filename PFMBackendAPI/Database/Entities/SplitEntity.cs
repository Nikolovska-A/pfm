using System;
using System.Text.Json.Serialization;

namespace PFMBackendAPI.Database.Entities
{
	public class SplitEntity
	{

        public int SplitId { get; set; }
        public string Description { get; set; }
        public string CatCode { get; set; }
        public double Amount { get; set; }
        public int TransactionId { get; set; }
        public TransactionEntity Transaction { get; set; }

        public SplitEntity()
		{
		}
	}
}

