using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.CredentialDtos
{
    public class CreadentialCreate
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CredentialFile { get; set; }
        public Guid ProductId { get; set; }
    }
}