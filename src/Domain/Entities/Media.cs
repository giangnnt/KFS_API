using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Domain.Entities
{
    public class Media
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}