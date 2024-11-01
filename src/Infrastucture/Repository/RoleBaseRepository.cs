using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;
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

        public async Task<bool> CreateRole(Role role)
        {
            _context.Roles.Add(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRole(int roleId)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == roleId);
            if (role == null) throw new Exception("Role not found");
            _context.Roles.Remove(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetPermissionRoleSlugs(int roleId)
        {
            return await _context.Roles.Where(x => x.RoleId == roleId)
                .Include(x => x.Permissions)
                .SelectMany(x => x.Permissions)
                .Select(x => x.Slug)
                .ToListAsync();
        }

        public async Task<Role> GetRoleById(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == roleId) ?? throw new Exception("Role not found");
        }

        public async Task<int> GetRoleId(string roleName)
        {
            return await Task.FromResult(_context.Roles.FirstOrDefault(x => x.Name == roleName)?.RoleId ?? throw new Exception("Role not found"));
        }

        public async Task<IEnumerable<User>> GetUsersByRole(int roleId)
        {
            return await _context.Users.Where(x => x.RoleId == roleId).ToListAsync();
        }

        public async Task<bool> UpdatePermissionRole(int roleId, List<string> permissionSlug)
        {
            var role = _context.Roles.FirstOrDefault(x => x.RoleId == roleId) ?? throw new Exception("Role not found");
            var permissions = _context.Permissions.Where(x => permissionSlug.Contains(x.Slug)).ToList();
            // remove old permission
            role.Permissions.Clear();
            // add new permission
            role.Permissions = permissions;
            _context.Roles.Update(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateUserRole(int roleId, Guid userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId) ?? throw new Exception("User not found");
            user.RoleId = roleId;
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}