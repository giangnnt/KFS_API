using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.CategoryDtos
{
    public class CategoryUpdate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }
}