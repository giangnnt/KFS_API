using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IPaymentRepository
    {
        Task<bool> CreatePayment(Payment payment);
        Task<IEnumerable<Payment>> GetPayments();
        Task<Payment> GetPaymentById(Guid id);
        Task<bool> UpdatePayment(Payment payment);
        Task<bool> DeletePayment(Payment payment);
    }
}