namespace KFS.src.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
        public string? Image { get; set; }
    }
}