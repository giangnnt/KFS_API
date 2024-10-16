using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsBatch { get; set; } = false;
        public bool IsConsignment { get; set; } = false;
        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}