using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Domain.Entities
{
    public class Consignment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ConsignmentMethodEnum Method { get; set; }
        public int? CommissionPercentage { get; set; }
        public int? DealingAmount { get; set; }
        public ConsignmentStatusEnum Status { get; set; }
        public bool IsForSell { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ConsignmentFee { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public User User { get; set; } = null!;
        public Product Product { get; set; } = new();
    }
}