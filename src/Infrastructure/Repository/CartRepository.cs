using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.infrastructure.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly KFSContext _context;
        public CartRepository(KFSContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRemoveCartItem(Cart cart)
        {
            DateTime nowVietnam = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            cart.UpdatedAt = nowVietnam;
            _context.Carts.Update(cart);

            foreach (var item in cart.CartItems)
            {
                //if item is not in the database
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    _context.CartItems.Attach(item);
                    _context.Entry(item).State = EntityState.Added;
                }
            }

            try
            {
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> CreateCart(Cart cart)
        {
            cart.CreatedAt = DateTime.Now;
            _context.Carts.Add(cart);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCart(Guid id)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.Id == id);
            if (cart == null) throw new Exception("Cart not found");
            _context.Carts.Remove(cart);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Cart> GetCartById(Guid id)
        {

            return await _context.Carts
            .Include(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Cart not found");
        }

        public async Task<IEnumerable<Cart>> GetCartByUserId(Guid userId)
        {
            return await _context.Carts
            .Include(x => x.CartItems)
            .Where(x => x.UserId == userId)
            .ToListAsync();
        }

        public async Task<IEnumerable<Cart>> GetCarts()
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ToListAsync();
        }

        public async Task<bool> UpdateCart(Cart cart)
        {
            DateTime nowVietnam = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            cart.UpdatedAt = nowVietnam; ;
            _context.Carts.Update(cart);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}