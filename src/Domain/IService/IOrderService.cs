using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IOrderService
    {
        Task<ResponseDto> GetOrders(OrderQuery req);
        Task<ResponseDto> GetOrderById(Guid id);
        Task<ResponseDto> CreateOrderFromCart(Guid id, OrderCreateFromCart req);
        Task<ResponseDto> UpdateOrder(OrderUpdate req, Guid id);
        Task<ResponseDto> AcceptOrder(Guid id, bool isAccept);
        Task<ResponseDto> DeleteOrder(Guid id);
        Task<ResponseDto> GetOrderByUserId(Guid userId);
        Task<ResponseDto> GetResponsePaymentUrl();
        Task<ResponseDto> CreateOrderOffline(OrderCreateOffline req);
        Task<ResponseDto> GetOwnOrder();
        Task<ResponseDto> CancelOrder(Guid id);
        Task<ResponseDto> OrderReturn(Guid orderId);
    }
}