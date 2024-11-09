using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Domain.Entities;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Domain.IRepository
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderById(Guid orderId);
        Task<ObjectPaging<Order>> GetOrders(OrderQuery orderQuery);
        Task<bool> CreateOrder(Order order);
        Task<bool> UpdateOrder(Order order);
        Task<bool> DeleteOrder(Guid orderId);
        Task<IEnumerable<Order>> GetOrderByUserId(Guid userId);
    }
}