using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Core.Jwt
{
    public class Payload
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public Guid SessionId { get; set; }
    }
}