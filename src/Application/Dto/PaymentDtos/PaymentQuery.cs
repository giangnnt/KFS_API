using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using KFS.src.Application.Enum;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.PaymentDtos
{
    public class PaymentQuery : PaginationReq
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum? PaymentMethod { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatusEnum? Status { get; set; }
        public DateTime? CreatedAtBefore { get; set; }
    }
}