using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> DeletePayment(Guid id)
        {
            var payment = await _context.Payments.FindAsync(id) ?? throw new Exception("Payment not found");
            _context.Payments.Remove(payment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Payment> GetPaymentById(Guid id)
        {
            return await _context.Payments.FindAsync(id) ?? throw new Exception("Payment not found");
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            return await _context.Payments
            .Include(x => x.Order)
            .ToListAsync();
        }

        public async Task<bool> UpdatePayment(Payment payment)
        {
            _context.Update(payment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}