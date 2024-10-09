using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.CartDtos
{
    public class BatchAddRemoveDto
    {
        public Guid CartId { get; set; }
        public Guid BatchId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
}