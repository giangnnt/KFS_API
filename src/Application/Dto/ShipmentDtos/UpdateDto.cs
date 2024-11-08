using KFS.src.Application.Enum;
using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.ShipmentDtos
{
    public class UpdateDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ShipmentStatusEnum Status { get; set; }
    }
}