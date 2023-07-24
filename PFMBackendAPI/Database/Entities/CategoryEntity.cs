using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Database.Entities
{
	public class CategoryEntity
	{

        public string Code { get; set; }
        public string ParentCode { get; set; }
        public string Name { get; set; }

        public List<TransactionEntity> Transactions { get; set; }

        public CategoryEntity()
		{
		}
	}
}

