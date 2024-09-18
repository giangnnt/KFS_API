using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;

namespace KFS.src.Infrastucture.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly KFSContext _context;
        public PaymentRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<bool> CreatePayment(Payment payment)
        {
            payment.CreatedAt = DateTime.Now;
            _context.Payments.Add(payment);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}