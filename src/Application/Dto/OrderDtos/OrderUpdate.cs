using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderUpdate
    {
        public string? ShippingAddress { get; set; }
        public string? ContactNumber { get; set; }
        public string? ContactName { get; set; }
        public string? Note { get; set; }
    }
}