using KFS.src.Application.Dto.OrderItemDtos;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderCreateOffline
    {
        public List<Guid>? ProductIds { get; set; }
        public List<Guid>? BatchIds { get; set; }
        public string? DiscountCode { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "VND";
    }
}