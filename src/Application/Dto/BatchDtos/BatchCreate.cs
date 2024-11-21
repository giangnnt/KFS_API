namespace KFS.src.Application.Dto.BatchDtos
{
    public class BatchCreate
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public List<Guid>? ProductIds { get; set; }
    }
}