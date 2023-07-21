using System;
using System.ComponentModel.DataAnnotations;
using PFMBackendAPI.Helpers;

namespace PFMBackendAPI.Models
{
	public class Transaction
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
        public Category Category { get; set; }

        public Transaction()
		{
		}

        public Transaction(TransactionCsvLine transactionCsvLine)
        {
            int id = 0;
            int.TryParse(transactionCsvLine.id, out id);

            DateTime date = new DateTime();
            DateTime.TryParse(transactionCsvLine.date, out date);

            char direction = ' ';
            char.TryParse(transactionCsvLine.direction, out direction);

            double amount = 0;
            double.TryParse(transactionCsvLine.amount, out amount);

            Currency currency;
            Enum.TryParse<Currency>(transactionCsvLine.currency, out currency);

            int mcc = 0;
            int.TryParse(transactionCsvLine.mcc, out mcc);

            this.TransactionId = id;
            this.BeneficiaryName = transactionCsvLine.beneficiaryname;
            this.Date = date;
            this.Direction = direction;
            this.Amount = amount;
            this.Description = transactionCsvLine.description;
            this.Currency = currency;
            this.Mcc = mcc;
            this.Kind = transactionCsvLine.kind;
        }
	}
}

