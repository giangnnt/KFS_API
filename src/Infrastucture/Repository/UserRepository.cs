using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;

namespace KFS.src.Infrastucture.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly KFSContext _context;
        public UserRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<User> GetUserById(Guid id)
        {
            return await _context.Users.FindAsync(id) ?? throw new Exception("User not found");
        }
    }
}