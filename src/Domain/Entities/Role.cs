namespace KFS.src.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new();
        public List<Permission> Permissions { get; set; } = new();
    }
}