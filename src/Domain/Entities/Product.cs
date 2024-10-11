using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }    
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Origin { get; set; }
        public int? Age { get; set; }
        public float? Length { get; set; }
        public string? Species { get; set; }
        public string? Color { get; set; }
        public string? FeedingVolumn { get; set; }
        public float? FilterRate { get; set; }
        public GenderEnum? Gender { get; set; }
        public int Inventory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ConsignmentId { get; set; }
        public bool IsForSell { get; set; } = false;
        public Category Category { get; set; } = null!;
        public Consignment? Consignment { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public List<Batch> Batches { get; set; } = new List<Batch>();
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
        public List<Media> Medias { get; set; } = new List<Media>();
    }
}