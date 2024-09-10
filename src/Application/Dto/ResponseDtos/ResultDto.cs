using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.ResponseDtos
{
    public class ResultDto
    {
        public required object Data { get; set; }
        public PageInfo? PageInfo { get; set; }
    }

    public class PageInfo : PaginationResp
    {
        public bool? HasNextPage { get; set; }
    }
}