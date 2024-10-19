using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Media
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public MediaTypeEnum Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}