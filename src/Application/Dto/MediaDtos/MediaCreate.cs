using KFS.src.Application.Enum;
using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.MediaDtos
{
    public class MediaCreate
    {
        public string? Url { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MediaTypeEnum Type { get; set; }
    }
}