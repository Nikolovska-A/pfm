using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Entities
{
	public class TransactionEntity
	{
        public int TransactionId { get; set; }
        public string BeneficiaryName { get; set; }
        public DateTime Date { get; set; }
        public char Direction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public Currency Currency { get; set; }
        public int Mcc { get; set; }
        public string Kind { get; set; }
        public string CatCode { get; set; }
        public CategoryEntity Category { get; set; }
        public List<SplitEntity> Splits { get; set; }

        public TransactionEntity()
		{
		}
	}
}

