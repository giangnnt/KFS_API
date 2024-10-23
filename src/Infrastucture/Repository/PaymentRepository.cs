using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.PaymentDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;
using static KFS.src.Application.Dto.Pagination.Pagination;

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

        public async Task<bool> DeletePayment(Payment payment)
        {
            _context.Payments.Remove(payment);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Payment> GetPaymentById(Guid id)
        {
            return await _context.Payments.FindAsync(id) ?? throw new Exception("Payment not found");
        }

        public async Task<ObjectPaging<Payment>> GetPayments(PaymentQuery paymentQuery)
        {
            var query = _context.Payments.AsQueryable();
            // search syntax
            query = query.Where(p => p.PaymentMethod == paymentQuery.PaymentMethod || paymentQuery.PaymentMethod == null);
            query = query.Where(p => p.Status == paymentQuery.Status || paymentQuery.Status == null);
            query = query.Where(p => p.CreatedAt <= paymentQuery.CreatedAtBefore || paymentQuery.CreatedAtBefore == null);
            // set total
            var total = await query.CountAsync();
            // Lấy các PaymentOrder và bao gồm Order
            var paymentOrders = await query
                .OfType<PaymentOrder>() // Chỉ lấy các PaymentOrder
                .Include(po => po.Order) // Include thông tin Order
                .ToListAsync();

            // Lấy các PaymentConsignment và bao gồm Consignment
            var paymentConsignments = await query
                .OfType<PaymentConsignment>() // Chỉ lấy các PaymentConsignment
                .Include(pc => pc.Consignment) // Include thông tin Consignment
                .ToListAsync();

            // Kết hợp cả hai loại Payment lại thành một danh sách duy nhất
            var allPayments = new List<Payment>();
            allPayments.AddRange(paymentOrders);
            allPayments.AddRange(paymentConsignments);

            return new ObjectPaging<Payment>
            {
                List = allPayments,
                Total = total
            };
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserId(Guid userId)
        {
            return await _context.Payments
                .Where(x => x.UserId == userId)
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