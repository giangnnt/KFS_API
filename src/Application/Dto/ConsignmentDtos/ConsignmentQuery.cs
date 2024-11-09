using KFS.src.Application.Enum;
using System.Text.Json.Serialization;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.ConsignmentDtos
{
    public class ConsignmentQuery : PaginationReq
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConsignmentStatusEnum? Status { get; set; }
        public bool? IsForSell { get; set; }
        public DateTime? ExpiryDateBefore { get; set; }
    }
}