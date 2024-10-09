using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Application.Dto.ShipmentDtos
{
    public class UpdateDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ShipmentStatusEnum Status { get; set; }
    }
}