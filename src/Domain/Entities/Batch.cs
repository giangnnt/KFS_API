using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Batch
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int Inventory { get; set; }
        public decimal Price { get; set; }
        public Guid ProductId { get; set; }
        public ProductStatusEnum Status { get; set; }
        public bool IsForSell { get; set; }
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
        public Product Product { get; set; } = null!;
    }
}