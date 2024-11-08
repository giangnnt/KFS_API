using System.ComponentModel.DataAnnotations;

namespace KFS.src.Domain.Entities
{
    public class Permission
    {
        [Key]
        public string Slug { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<Role> Roles { get; set; } = new();
    }
}