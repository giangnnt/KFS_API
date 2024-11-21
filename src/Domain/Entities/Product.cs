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
        public int Weight { get; set; }
        public float? Length { get; set; }
        public string? Color { get; set; }
        public string? FeedingVolumn { get; set; }
        public float? FilterRate { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ConsignmentId { get; set; }
        public Guid? BatchId { get; set; }
        public bool IsForSell { get; set; } = false;
        public ProductStatusEnum Status { get; set; }
        public string? ImageUrl { get; set; }
        public Category Category { get; set; } = null!;
        public Consignment? Consignment { get; set; }
        public List<CartItemProduct> CartItemProducts { get; set; } = new List<CartItemProduct>();
        public List<OrderItemProduct> OrderItems { get; set; } = new List<OrderItemProduct>();
        public Batch? Batch { get; set; }
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
        public List<Media> Medias { get; set; } = new List<Media>();
        public Credential? Credential { get; set; }
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
    public class ProductResponse
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public int Total { get; set; }
    }
}