using System;
using PFMBackendAPI.Models.Responses;

namespace PFMBackendAPI.Models.dto
{
    public class ErrorResponseDtoWithRow : ErrorResponseDto
    {
        public int Row { get; set; }

        public ErrorResponseDtoWithRow(string tag, string error, string message, int row)
            : base(tag, error, message)
        {
            this.Row = row;
        }
    }

}

