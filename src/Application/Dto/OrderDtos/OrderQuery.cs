using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderQuery : PaginationReq
    {
        public PaymentMethodEnum? PaymentMethod { get; set; }
        public OrderStatusEnum? Status { get; set; }
    }
}