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
        public class ObjectPaging<T>
        {
            public List<T> List { get; set; } = new List<T>();
            public int Total { get; set; }
        }
    }
}