using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Dto.FeedbackDtos
{
    public class FeedbackQuery : PaginationReq
    {
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int? Rating { get; set; }
        public Guid? ProductId { get; set; }
    }
}