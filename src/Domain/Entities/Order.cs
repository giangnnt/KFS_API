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
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public int ShippingFee { get; set; }
        public float Discount { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "VND";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}