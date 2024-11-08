using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.ResponseDtos
{
    public class ResultDto
    {
        public required object Data { get; set; }
        public PaginationResp? PaginationResp { get; set; }
    }
}