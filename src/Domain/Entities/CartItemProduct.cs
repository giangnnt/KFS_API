namespace KFS.src.Domain.Entities
{
    public class CartItemProduct : CartItem
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
