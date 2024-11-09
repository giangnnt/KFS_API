namespace KFS.src.Application.Dto.ConsignmentDtos
{
    public class ConsignmentCreateByOrderItem
    {
        public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public int CommissionPercentage { get; set; }
        public int ConsignmentFee { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DealingAmount { get; set; }
        public bool IsForSell { get; set; }
    }
}