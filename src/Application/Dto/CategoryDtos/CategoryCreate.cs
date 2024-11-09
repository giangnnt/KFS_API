namespace KFS.src.Application.Dto.CategoryDtos
{
    public class CategoryCreate
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}