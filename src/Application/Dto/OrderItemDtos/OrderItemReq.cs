namespace KFS.src.Application.Dto.OrderItemDtos
{
    public class OrderItemProductReq
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderItemBatchReq
    {
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
    }
}