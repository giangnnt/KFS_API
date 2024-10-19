using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.CredentialDtos
{
    public class CredentialUpdate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CredentialFile { get; set; }
    }
}