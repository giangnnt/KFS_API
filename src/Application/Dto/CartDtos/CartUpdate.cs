using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Application.Dto.CartDtos
{
    public class CartUpdate
    {
        public string Currency { get; set; } = "VND";
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CartStatusEnum Status { get; set; }
    }
}