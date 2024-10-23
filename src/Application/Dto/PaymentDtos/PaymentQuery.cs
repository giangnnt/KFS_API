using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.PaymentDtos
{
    public class PaymentQuery : PaginationReq
    {
        public PaymentMethodEnum? PaymentMethod { get; set; }
        public PaymentStatusEnum? Status { get; set; }
        public DateTime? CreatedAtBefore { get; set; }
    }
}