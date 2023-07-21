using System;
namespace PFMBackendAPI.Models.Responses
{
	public class SpendingAnalyticsResponse
	{
		public List<GroupAnalyticsDto> groups { get; set; }

		public SpendingAnalyticsResponse(List<GroupAnalyticsDto> groups)
		{
			this.groups = groups;
		}
	}
}

