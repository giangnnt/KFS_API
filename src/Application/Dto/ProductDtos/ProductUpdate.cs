using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Application.Dto.ProductDtos
{
    public class ProductUpdate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Origin { get; set; }
        public int? Age { get; set; }
        public float? Length { get; set; }
        public string? Species { get; set; }
        public string? Color { get; set; }
        public string? FeedingVolumn { get; set; }
        public float? FilterRate { get; set; }
        public GenderEnum? Gender { get; set; }
        public int? Inventory { get; set; }
        public Guid? CategoryId { get; set; }
    }
}