using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.ConsignmentDtos
{
    public class ConsignmentCreateByOrderItem
    {
        public Guid OrderId { get; set; }
        public Guid OrderItemId { get; set; }
        public int CommissionPercentage { get; set; }
        public int ConsignmentFee { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public int DealingAmount { get; set; }
        public bool IsForSell { get; set; }
    }
}