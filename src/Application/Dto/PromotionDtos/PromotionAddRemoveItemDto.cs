using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.PromotionDtos
{
    public class PromotionAddRemoveItemDto
    {
        public List<Guid> listId { get; set; } = new List<Guid>();
    }
}