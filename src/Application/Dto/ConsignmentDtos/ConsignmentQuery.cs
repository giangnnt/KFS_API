using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.ConsignmentDtos
{
    public class ConsignmentQuery : PaginationReq
    {
        public ConsignmentStatusEnum? Status { get; set; }
        public bool? IsForSell { get; set; }
        public DateTime? ExpiryDateBefore { get; set; }
    }
}