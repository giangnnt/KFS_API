using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetCarts();
        Task<Cart> GetCartById(Guid id);
        Task<bool> CreateCart(Cart cart);
        Task<bool> UpdateCart(Cart cart);
        Task<bool> DeleteCart(Guid id);
    }
}