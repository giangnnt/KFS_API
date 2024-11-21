namespace KFS.src.Domain.Entities
{
    public class OrderItemProduct : OrderItem
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
