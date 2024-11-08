using KFS.src.Application.Dto.PaymentDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IPaymentService
    {
        Task<ResponseDto> GetPaymentById(Guid id);
        Task<ResponseDto> GetPayments(PaymentQuery paymentQuery);
        Task<ResponseDto> CreatePaymentOffline(Guid id);
        Task<ResponseDto> DeletePayment(Guid id);
        Task<ResponseDto> CreatePaymentByOrderIdCOD(Guid orderId);
        Task<ResponseDto> GetPaymentByUser(Guid userId);
        Task<ResponseDto> GetOwnPayment();
    }
}