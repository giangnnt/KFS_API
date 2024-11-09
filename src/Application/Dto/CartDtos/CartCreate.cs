using KFS.src.Application.Enum;
using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.CartDtos
{
    public class CartCreate
    {
        public string Currency { get; set; } = "VND";
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CartStatusEnum Status { get; set; }
    }
}