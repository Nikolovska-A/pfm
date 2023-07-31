using System;
namespace PFMBackendAPI.Models.dto
{
	public class StatisticsDto
	{
        public double amount { get; set; }
        public string catCode { get; set; }
        public string categoryName { get; set; }

        public StatisticsDto()
		{
		}
	}
}

