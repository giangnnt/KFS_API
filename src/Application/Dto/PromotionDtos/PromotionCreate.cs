using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.PromotionDtos
{
    public class PromotionCreate
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? DiscountCode { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}