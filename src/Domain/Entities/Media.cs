using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Media
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = null!;
        public MediaTypeEnum Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Credential> Credentials { get; set; } = new List<Credential>();
    }
}