using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IOrderItemRepository
    {
        Task<bool> CreateOrderItem(OrderItem orderItem);
        Task<bool> UpdateOrderItem(OrderItem orderItem);
        Task<bool> DeleteOrderItem(Guid id);
        Task<OrderItem> GetOrderItemById(Guid id);
        Task<IEnumerable<OrderItem>> GetOrderItems();
    }
}