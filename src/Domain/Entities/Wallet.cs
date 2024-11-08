namespace KFS.src.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public int Point { get; set; }
        public Guid UserId { get; set; }
        public User Owner { get; set; } = null!;
    }
}