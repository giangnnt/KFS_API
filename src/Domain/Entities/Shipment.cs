using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ShipperId { get; set; }
        public ShipmentStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Order Order { get; set; } = null!;
        public User Shipper { get; set; } = null!;
    }
}