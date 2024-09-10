using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.ResponseDtos
{
    public class ResponseDto
    {
        public int StatusCode { get; set; } = 200;
        public string? Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
        public ResultDto? Result { get; set; }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}