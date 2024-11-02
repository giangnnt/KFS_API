using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.RoleDtos
{
    public class RoleCreate
    {
        public string RoleName { get; set; } = null!;
        public string? Description { get; set; }
    }
}