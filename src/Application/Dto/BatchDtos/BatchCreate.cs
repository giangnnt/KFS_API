using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.BatchDtos
{
    public class BatchCreate
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int NumberOfBatchs { get; set; }
        public decimal Price { get; set; }
    }
}