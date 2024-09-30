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
    public class UserRepository : IUserRepository
    {
        private readonly KFSContext _context;
        public UserRepository(KFSContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email)
            .Include(x => x.Password)
            .FirstOrDefaultAsync() ?? throw new Exception("User not found");
        }

        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.FindAsync(id) ?? throw new Exception("User not found");
        }
    }
}