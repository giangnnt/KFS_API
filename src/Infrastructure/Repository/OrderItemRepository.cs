using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.infrastructure.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly KFSContext _context;
        public OrderItemRepository(KFSContext context)
        {
            _context = context;
        }
        public Task<bool> CreateOrderItem(OrderItem orderItem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOrderItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderItem> GetOrderItemById(Guid id)
        {
            return await _context.OrderItems
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("OrderItem not found");
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItems()
        {
            return await _context.OrderItems
            .Include(x => x.Product)
            .ToListAsync();
        }

        public async Task<bool> UpdateOrderItem(OrderItem orderItem)
        {

            _context.OrderItems.Update(orderItem);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}