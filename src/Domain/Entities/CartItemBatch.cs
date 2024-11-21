namespace KFS.src.Domain.Entities
{
    public class CartItemBatch : CartItem
    {
        public Guid BatchId { get; set; }
        public Batch Batch { get; set; } = null!;
    }
}
