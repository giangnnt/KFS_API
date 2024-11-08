using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderUpdateStatus
    {
        public Guid Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public bool IsAccept { get; set; }
    }
}