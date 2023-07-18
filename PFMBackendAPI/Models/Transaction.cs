using System;
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

        public Transaction()
		{
		}

        public Transaction(CsvLine csvLine)
        {
            int id = 0;
            int.TryParse(csvLine.id, out id);

            DateTime date = new DateTime();
            DateTime.TryParse(csvLine.date, out date);

            char direction = ' ';
            char.TryParse(csvLine.direction, out direction);

            double amount = 0;
            double.TryParse(csvLine.amount, out amount);

            Currency currency;
            Enum.TryParse<Currency>(csvLine.currency, out currency);

            int mcc = 0;
            int.TryParse(csvLine.mcc, out mcc);

            this.TransactionId = id;
            this.BeneficiaryName = csvLine.beneficiaryname;
            this.Date = date;
            this.Direction = direction;
            this.Amount = amount;
            this.Description = csvLine.description;
            this.Currency = currency;
            this.Mcc = mcc;
            this.Kind = csvLine.kind;
        }
	}
}

