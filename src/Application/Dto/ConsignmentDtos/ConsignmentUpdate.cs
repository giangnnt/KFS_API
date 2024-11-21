namespace KFS.src.Application.Dto.ConsignmentDtos
{
    public class ConsignmentUpdate
    {
        public int? CommissionPercentage { get; set; }
        public int? DealingAmount { get; set; }
        public int ConsignmentFee { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? ImageUrl { get; set; }
    }
}