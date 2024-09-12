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
    public class CartRepository : ICartRepository
    {
        private readonly KFSContext _context;
        public CartRepository(KFSContext context)
        {
            _context = context;
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
            if(cart == null) throw new Exception("Cart not found");
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

        public async Task<IEnumerable<Cart>> GetCarts()
        {
            return await _context.Carts
            .Include(x => x.CartItems)
            .ToListAsync();
        }

        public async Task<bool> UpdateCart(Cart cart)
        {
            cart.UpdatedAt = DateTime.Now;
            _context.Carts.Update(cart); // Đánh dấu giỏ hàng là đã thay đổi
        // Đảm bảo các mục giỏ hàng cũng được theo dõi
        foreach (var item in cart.CartItems)
        {
            _context.Add(item);
            _context.Entry(item).State = EntityState.Modified;
        }
        try
        {
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (DbUpdateConcurrencyException)
        {
            // Xử lý lỗi đồng bộ nếu cần
            return false;
        }
        }
    }
}