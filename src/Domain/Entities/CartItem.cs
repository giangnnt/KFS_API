using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsBatch { get; set; } = false;
        public Cart Cart { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}