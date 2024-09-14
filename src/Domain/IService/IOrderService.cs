using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IOrderService
    {
        Task<ResponseDto> GetOrders();
        Task<ResponseDto> GetOrderById(Guid id);
        Task<ResponseDto> CreateOrderFromCart(OrderCreateFromCart req);
        Task<ResponseDto> UpdateOrder(OrderUpdate req, Guid id);
        Task<ResponseDto> DeleteOrder(Guid id);
        Task<ResponseDto> GetOrderByUserId(Guid userId);
        Task<ResponseDto> AcceptOrder(Guid id);
    }
}