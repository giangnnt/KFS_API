using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public int TotalWeight { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalItem { get; set; }
        public string? ContactNumber { get; set; }
        public string? ContactName { get; set; }
        public int ShippingFee { get; set; }
        public int Discount { get; set; }
        public bool IsUsePoint { get; set; } = false;
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string? EstimatedDeliveryDate { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "VND";
        public OrderStatusEnum Status { get; set; }
        public bool IsReBuy { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User User { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public PaymentOrder? Payment { get; set; }
        public Shipment? Shipment { get; set; }
        public Address? Address { get; set; }
    }
}