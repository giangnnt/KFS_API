using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.FeedbackDtos
{
    public class FeedbackUpdate
    {
        public string? Description { get; set; }
        public int? Rating { get; set; }
    }
}