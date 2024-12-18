using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.infrastructure.Repository
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
            return await _context.CartItems.FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("cart item not found");
        }

        public async Task<bool> UpdateCartItem(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}