namespace KFS.src.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Price { get; set; }
        public bool IsConsignment { get; set; } = false;
        public Order Order { get; set; } = null!;
    }
}