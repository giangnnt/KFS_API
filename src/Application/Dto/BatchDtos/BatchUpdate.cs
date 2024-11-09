namespace KFS.src.Application.Dto.BatchDtos
{
    public class BatchUpdate
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int Inventory { get; set; }
        public decimal Price { get; set; }
    }
}