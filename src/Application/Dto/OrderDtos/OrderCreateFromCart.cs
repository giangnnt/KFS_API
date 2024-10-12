using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderCreateFromCart
    {
        public Guid CartId { get; set; }
        public string ShippingAddress { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string? DiscountCode { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "VND";
        public bool UsePoint { get; set; } = false;
    }
}