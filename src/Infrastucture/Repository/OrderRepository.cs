using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Infrastucture.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly KFSContext _context;
        public OrderRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateOrder(Order order)
        {
            order.CreatedAt = DateTime.Now;
            _context.Orders.Add(order);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteOrder(Guid orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            if (order == null) throw new Exception("Order not found");
            _context.Orders.Remove(order);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            return await _context.Orders
            .Include(x => x.OrderItems)
            .Include(x => x.Payment)
            .Include(x => x.Shipment)
            .FirstOrDefaultAsync(x => x.Id == orderId) ?? throw new Exception("Order not found");   
        }

        public async Task<IEnumerable<Order>> GetOrderByUserId(Guid userId)
        {
            return await _context.Orders
            .Include(x => x.OrderItems)
            .Include(x => x.Payment)
            .Include(x => x.Shipment)
            .Where(x => x.UserId == userId)
            .ToListAsync();
        }

        public async Task<ObjectPaging<Order>> GetOrders(OrderQuery orderQuery)
        {
            var query = _context.Orders.AsQueryable();
            // search syntax
            query = query.Where(p => p.PaymentMethod == orderQuery.PaymentMethod || orderQuery.PaymentMethod == null);
            query = query.Where(p => p.Status == orderQuery.Status || orderQuery.Status == null);
            //set total
            var total = await query.CountAsync();

            // return
            var orderList = await query
            .Include(x => x.OrderItems)
            .Include(x => x.Payment)
            .Include(x => x.Shipment)
            .ToListAsync();
            return new ObjectPaging<Order>
            {
                List = orderList,
                Total = total
            };
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            DateTime nowVietnam = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            order.UpdatedAt = nowVietnam;
            _context.Orders.Update(order);

            foreach (var item in order.OrderItems)
            {
                //if item is not in the database
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    _context.OrderItems.Add(item);
                }
                else
                {
                    _context.OrderItems.Update(item);
                }
            }

            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}