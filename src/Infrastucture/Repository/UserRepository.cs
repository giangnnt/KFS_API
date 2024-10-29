using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.UserDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Infrastucture.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly KFSContext _context;
        public UserRepository(KFSContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(User user)
        {
            user.CreatedAt = DateTime.Now;
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email)
            .Include(x => x.Role)
            .Include(x => x.Address)
            .Include(x => x.Wallet)
            .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _context.Users
            .Include(x => x.Role)
            .Include(x => x.Address)
            .Include(x => x.Wallet)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("User not found");
        }

        public async Task<ObjectPaging<User>> GetUsersAdmin(UserQuery userQuery)
        {
            var query = _context.Users.AsQueryable();
            // search
            query = query.Where(p => p.RoleId == userQuery.RoleId || userQuery.RoleId == null);
            query = query.Where(p => p.IsActive == userQuery.IsActive || userQuery.IsActive == null);
            // total
            var total = await query.CountAsync();

            var userList = await query
            .Include(x => x.Role)
            .Include(x => x.Address)
            .Include(x => x.Wallet)
            .Skip((userQuery.Page - 1) * userQuery.PageSize)
            .Take(userQuery.PageSize)
            .ToListAsync();
            return new ObjectPaging<User>
            {
                List = userList,
                Total = total
            };
        }

        public async Task<bool> UpdateUser(User user)
        {
            _context.Users.Update(user);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}