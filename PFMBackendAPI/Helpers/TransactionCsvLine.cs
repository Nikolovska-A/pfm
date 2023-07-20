using System;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Helpers
{
	public class TransactionCsvLine
	{

        public string id { get; set; }
        public string beneficiaryname { get; set; }
        public string date { get; set; }
        public string direction { get; set; }
        public string amount { get; set; }
        public string description { get; set; }
        public string currency { get; set; }
        public string mcc { get; set; }
        public string kind { get; set; }


        public TransactionCsvLine()
		{
		}
	}
}

