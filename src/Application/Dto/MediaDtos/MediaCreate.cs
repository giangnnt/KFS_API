using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Enum;

namespace KFS.src.Application.Dto.MediaDtos
{
    public class MediaCreate
    {
        public string? Url { get; set; }
        public MediaTypeEnum Type { get; set; }
    }
}