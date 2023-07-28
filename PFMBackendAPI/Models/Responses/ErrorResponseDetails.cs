﻿using System;
namespace PFMBackendAPI.Helpers
{
	public class ErrorResponseDetails
	{

        public int? statusCode { get; set; }
		public string? message { get; set; }
        public List<IDictionary<string, string[]>> errors { get; set; }
      
        public ErrorResponseDetails()
		{
		}
	}
}

