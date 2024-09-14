using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderCreateDto
    {
        public Guid UserId { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public int ShippingFee { get; set; }
        public float Discount { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "VND";
    }
}