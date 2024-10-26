using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Application.Dto.MediaDtos
{
    public class MediaCreate
    {
        public string? Url { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaTypeEnum Type { get; set; }
    }
}