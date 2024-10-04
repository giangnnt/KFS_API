using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.ConsignmentDtos
{
    public class ConsignmentCreate
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public int CommissionPercentage { get; set; }
        public int DealingAmount { get; set; }
    }
}