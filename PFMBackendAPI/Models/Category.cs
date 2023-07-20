using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Helpers;

namespace PFMBackendAPI.Models
{
	public class Category
	{
        public string CodeId { get; set; }
        public string ParentCode { get; set; }
        public string Name { get; set; }

		public List<Transaction> Transactions { get; set; }


		public Category toCategory (CategoryCsvLine categoryCsvLine)
		{
			Category category = new Category();
			category.CodeId = categoryCsvLine.code;
            category.ParentCode = categoryCsvLine.parentcode;
            category.Name = categoryCsvLine.name;
			return category;

        }
	}
}

