using KFS.src.Application.Enum;
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
            var cartItemProduct = await _context.CartItems
                .Where(x => x.CartId == id)
                .OfType<CartItemProduct>()
                .Include(x => x.Product)
                .ToListAsync();
            var cartItemBatch = await _context.CartItems
                .Where(x => x.CartId == id)
                .OfType<CartItemBatch>()
                .Include(x => x.Batch)
                .ToListAsync();
            var cartItems = new List<CartItem>();
            cartItems.AddRange(cartItemProduct);
            cartItems.AddRange(cartItemBatch);
            var cart = await _context.Carts
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Cart not found");
            cart.CartItems = cartItems;
            return cart;
        }

        public async Task<IEnumerable<Cart>> GetCartByUserId(Guid userId)
        {
            var carts =  await _context.Carts
            .Include(x => x.CartItems)
            .Where(x => x.UserId == userId)
            .ToListAsync();
            foreach (var cart in carts)
            {
                var cartItemProduct = await _context.CartItems
                    .Where(x => x.CartId == cart.Id)
                    .OfType<CartItemProduct>()
                    .Include(x => x.Product)
                    .ToListAsync();
                var cartItemBatch = await _context.CartItems
                    .Where(x => x.CartId == cart.Id)
                    .OfType<CartItemBatch>()
                    .Include(x => x.Batch)
                    .ToListAsync();
                var cartItems = new List<CartItem>();
                cartItems.AddRange(cartItemProduct);
                cartItems.AddRange(cartItemBatch);
                cart.CartItems = cartItems;
            }
            return carts;
        }

        public async Task<IEnumerable<Cart>> GetCarts()
        {
            var carts = await _context.Carts.AsQueryable()
                .Include(x => x.CartItems)
                .ToListAsync();
            foreach (var cart in carts)
            {
                var cartItemProduct = await _context.CartItems
                    .Where(x => x.CartId == cart.Id)
                    .OfType<CartItemProduct>()
                    .Include(x => x.Product)
                    .ToListAsync();
                var cartItemBatch = await _context.CartItems
                    .Where(x => x.CartId == cart.Id)
                    .OfType<CartItemBatch>()
                    .Include(x => x.Batch)
                    .ToListAsync();
                var cartItems = new List<CartItem>();
                cartItems.AddRange(cartItemProduct);
                cartItems.AddRange(cartItemBatch);
                cart.CartItems = cartItems;
            }
            return carts;
        }

        public async Task<IEnumerable<Cart>> GetOwnCartCurrentActive(Guid userId)
        {
            var carts = await _context.Carts.AsQueryable()
                .Include(x => x.CartItems)
                .Where(x => x.UserId == userId)
                .Where(x => x.Status == CartStatusEnum.Active)
                .ToListAsync();
            foreach (var cart in carts)
            {
                var cartItemProduct = await _context.CartItems
                    .Where(x => x.CartId == cart.Id)
                    .OfType<CartItemProduct>()
                    .Include(x => x.Product)
                    .ToListAsync();
                var cartItemBatch = await _context.CartItems
                    .Where(x => x.CartId == cart.Id)
                    .OfType<CartItemBatch>()
                    .Include(x => x.Batch)
                    .ToListAsync();
                var cartItems = new List<CartItem>();
                cartItems.AddRange(cartItemProduct);
                cartItems.AddRange(cartItemBatch);
                cart.CartItems = cartItems;
            }
            return carts;
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