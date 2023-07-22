using System;
namespace PFMBackendAPI.Models.Responses
{
	[Serializable]
	public class GroupAnalyticsDto
    {
		public string catcode { get; set; }
		public double amount { get; set; }
		public int count { get; set; }

        public GroupAnalyticsDto()
		{
		}

        public GroupAnalyticsDto(string catcode, double amount, int count)
        {
			this.catcode = catcode;
			this.amount = amount;
			this.count = count;
        }
    }
}

