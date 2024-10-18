using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Domain.Entities
{
    public class PaymentOrder : Payment
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}