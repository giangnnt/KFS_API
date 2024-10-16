using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Domain.Entities
{
    public class PaymentConsignment : Payment
    {
        public Guid ConsignmentId { get; set; }
        public Consignment Consignment { get; set; } = null!;
    }
}