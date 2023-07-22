using System;
namespace PFMBackendAPI.Models.Responses.dto
{
	public class SplitDto
	{
		public string catcode { get; set; }
		public double amount { get; set; }
        public string Description { get; set; }

        public SplitDto()
		{
		}
	}
}

