using KFS.src.Application.Enum;
using System.Text.Json.Serialization;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderQuery : PaginationReq
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum? PaymentMethod { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum? Status { get; set; }
    }
}