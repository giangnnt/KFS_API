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
    public class CartItemRepository : ICartItemRepository
    {
        private readonly KFSContext _context;
        public CartItemRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCartItem(Guid id)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(x => x.Id == id);
            if (cartItem == null) throw new Exception("Cart item not found");
            _context.CartItems.Remove(cartItem);
            int result = _context.SaveChanges();
            return result > 0;
        }

        public async Task<CartItem> GetCartItemById(Guid id)
        {
            return await _context.CartItems
            .Include(x => x.Product)
            .Include(x => x.Cart)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Cart item not found");
        }

        public async Task<IEnumerable<CartItem>> GetCartItems()
        {
            return await _context.CartItems
            .Include(x => x.Product)
            .Include(x => x.Cart)
            .ToListAsync();
        }

        public async Task<bool> UpdateCartItem(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}