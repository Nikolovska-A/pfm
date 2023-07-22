using System;
using PFMBackendAPI.Models.Responses.dto;

namespace PFMBackendAPI.Models.Responses
{
	public class SplitRequest
	{
		public List<SplitDto> splits { get; set; }

		public SplitRequest()
		{
		}
	}
}

