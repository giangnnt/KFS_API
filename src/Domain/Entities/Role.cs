using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Infrastucture.Repository;

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