namespace KFS.src.Domain.Entities
{
    public class Credential
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CredentialFile { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public List<Media> Medias { get; set; } = new List<Media>();
    }
}