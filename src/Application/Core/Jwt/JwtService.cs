using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Core.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, Guid sessionId, int roleId, int exp);
        Payload? ValidateToken(string token);
    }
    public class JwtService : IJwtService
    {
        public string GenerateToken(Guid userId, Guid sessionId, int roleId, int exp)
        {
            throw new NotImplementedException();
        }

        public Payload? ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}