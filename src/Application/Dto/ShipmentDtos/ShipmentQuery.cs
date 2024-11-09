using KFS.src.Application.Enum;
using System.Text.Json.Serialization;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.ShipmentDtos
{
    public class ShipmentQuery : PaginationReq
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ShipmentStatusEnum? Status { get; set; }
    }
}