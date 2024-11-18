namespace KFS.src.Domain.Entities
{
    public class OrderItemBatch : OrderItem
    {
        public Guid BatchId { get; set; }
        public Batch Batch { get; set; } = null!;
    }
}
