using KFS.src.Application.Enum;
using System.Text.Json.Serialization;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.ProductDtos
{
    public class ProductQuery : PaginationReq
    {
        public string? Name { get; set; }
        public decimal PriceStart { get; set; }
        public decimal PriceEnd { get; set; }
        public string? Origin { get; set; }
        public string? Species { get; set; }
    }
    public class ProductAdminQuery : PaginationReq
    {
        public string? Name { get; set; }
        public decimal PriceStart { get; set; }
        public decimal PriceEnd { get; set; }
        public string? Origin { get; set; }
        public string? Species { get; set; }
        public bool? IsForSell { get; set; }
        public int? InventoryLowerThan { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductStatusEnum? Status { get; set; }
    }
}