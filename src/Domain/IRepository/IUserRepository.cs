using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByEmail(string email);
        Task<bool> CreateUser(User user);
        Task<List<User>>GetAllUser();
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(Guid id);
        Task<List<Order>> GetAllOrdersOfUserById();

    }
}