using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.OrderItemDtos;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderCreateOffline
    {
        public List<OrderItemProductReq>? ProductsReq { get; set; }
        public List<OrderItemBatchReq>? BatchesReq { get; set; }
        public string? DiscountCode { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "VND";
    }
}