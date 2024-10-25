using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Domain.Entities
{
    public class Promotion
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? DiscountCode { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Batch> Batches { get; set; } = new List<Batch>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}