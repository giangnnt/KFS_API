using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastucture.Repository
{
    public class RoleBaseRepository : IRoleBaseRepository
    {
        private readonly KFSContext _context;
        public RoleBaseRepository(KFSContext context)
        {
            _context = context;
        }
        public IEnumerable<string> GetPermissionRoleSlugs(int roleId)
        {
            return _context.Roles.Where(x => x.RoleId == roleId)
                .Include(x => x.Permissions)
                .SelectMany(x => x.Permissions)
                .Select(x => x.Slug)
                .ToList();
        }
    }
}