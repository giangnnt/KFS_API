using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalItem { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public int ShippingFee { get; set; }
        public float Discount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "VND";
        public OrderStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Payment? Payment { get; set; }
    }
}