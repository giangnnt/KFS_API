namespace KFS.src.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public decimal Price { get; set; }
        public int Weight { get; set; }
        public Cart Cart { get; set; } = null!;
    }
}