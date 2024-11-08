using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalItem { get; set; }
        public int TotalWeight { get; set; }
        public string Currency { get; set; } = "VND";
        public CartStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User User { get; set; } = null!;
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }

}