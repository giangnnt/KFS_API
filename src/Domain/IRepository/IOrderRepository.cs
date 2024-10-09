using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderById(Guid orderId);
        Task<IEnumerable<Order>> GetOrders();
        Task<bool> CreateOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(Guid orderId);
        Task<IEnumerable<Order>> GetOrderByUserId(Guid userId);
    }
}