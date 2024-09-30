using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Dto.Session
{
    public class RedisSession
    {
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public string? Refresh { get; set; }
    }

    public class LogoutReq
    {
        public required string RefreshToken { get; set; }
    }
}