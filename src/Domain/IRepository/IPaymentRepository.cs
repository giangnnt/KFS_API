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
    }
}