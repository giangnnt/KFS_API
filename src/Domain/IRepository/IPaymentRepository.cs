using KFS.src.Application.Dto.PaymentDtos;
using KFS.src.Domain.Entities;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Domain.IRepository
{
    public interface IPaymentRepository
    {
        Task<bool> CreatePayment(Payment payment);
        Task<ObjectPaging<Payment>> GetPayments(PaymentQuery query);
        Task<Payment> GetPaymentById(Guid id);
        Task<bool> UpdatePayment(Payment payment);
        Task<bool> DeletePayment(Payment payment);
        Task<IEnumerable<Payment>> GetPaymentsByUserId(Guid userId);
    }
}