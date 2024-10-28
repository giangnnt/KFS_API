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
        public Guid AddressId { get; set; }
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public int TotalWeight { get; set; }
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