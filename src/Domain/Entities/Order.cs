using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalItem { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public int ShippingFee { get; set; }
        public int Discount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string? EstimatedDeliveryDate { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "VND";
        public OrderStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User User { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Payment? Payment { get; set; }
        public Shipment Shipment { get; set; } = new();

    }
}