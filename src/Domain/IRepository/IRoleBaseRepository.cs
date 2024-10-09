using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Domain.IRepository
{
    public interface IRoleBaseRepository
    {
        public IEnumerable<string> GetPermissionRoleSlugs(int roleId);
    }
}