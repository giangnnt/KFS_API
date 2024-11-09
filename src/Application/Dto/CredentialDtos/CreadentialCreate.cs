namespace KFS.src.Application.Dto.CredentialDtos
{
    public class credentialCreate
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CredentialFile { get; set; }
        public Guid ProductId { get; set; }
    }
}