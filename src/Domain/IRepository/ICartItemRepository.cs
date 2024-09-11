using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetCartItems();
        Task<CartItem> GetCartItemById(Guid id);
        Task<bool> CreateCartItem(CartItem cartItem);
        Task<bool> UpdateCartItem(CartItem cartItem);
        Task<bool> DeleteCartItem(Guid id);
    }
}