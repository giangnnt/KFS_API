using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.UserDtos
{
    public class UserQuery : PaginationReq
    {
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}