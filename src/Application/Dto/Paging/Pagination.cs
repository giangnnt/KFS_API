using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.Pagination
{
    public class Pagination
    {
        public class PaginationReq
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }

    public class PaginationResp
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
    }
}