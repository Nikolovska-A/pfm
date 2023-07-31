using System;
using PFMBackendAPI.Models.dto;

namespace PFMBackendAPI.Models.Responses
{
	public class StatisticsResponse
	{

		public List<StatisticsDto> statistics { get; set; }

		public StatisticsResponse()
		{

		}

		public StatisticsResponse(List<StatisticsDto> statistics)
		{
			this.statistics = statistics;
		}
	}
}

