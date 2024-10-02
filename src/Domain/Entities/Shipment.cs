using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public ShipmentStatusEnum Status { get; set; }
        public Order Order { get; set; } = null!;
    }
}