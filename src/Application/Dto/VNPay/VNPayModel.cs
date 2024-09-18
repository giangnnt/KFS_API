using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.VNPay
{
    public class VNPayResponseModel
    {
        public string PaymentId { get; set; } = null!;
        public bool Success { get; set; }
        public string TransactionStatus { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        public string Amount { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public string OrderDesription { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string VnPayResponseCode { get; set; } = null!;
    }
    public class VNPayRequestModel
    {
        public Guid OrderId { get; set; }
        public string FullName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Amount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}