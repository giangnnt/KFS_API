using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.UserDtos
{
    public class UserQuery : PaginationReq
    {
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}