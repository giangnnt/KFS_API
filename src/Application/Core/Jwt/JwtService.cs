using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace KFS.src.Application.Core.Jwt
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, Guid sessionId, int roleId, int exp);
        Payload? ValidateToken(string token);
    }
    public class JwtService : IJwtService
    {
        private readonly byte[] _key;
        private readonly JwtSecurityTokenHandler _handler;
        public JwtService()
        {
            var SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "Koifarmshop@@";
            _key = System.Text.Encoding.ASCII.GetBytes(SecretKey);
            _handler = new JwtSecurityTokenHandler();
        }
        public string GenerateToken(Guid userId, Guid sessionId, int roleId, int exp)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new("sessionId", sessionId.ToString()),
                    new("roleId", roleId.ToString())
                ]),
                Issuer = userId.ToString(),
                Expires = DateTime.UtcNow.AddSeconds(exp),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _handler.CreateToken(tokenDescriptor);

            return _handler.WriteToken(token);
        }

        public Payload? ValidateToken(string token)
        {
            _handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(_key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var result = (JwtSecurityToken)validatedToken;

            var payload = new Payload()
            {
                UserId = Guid.Parse(result.Issuer),
                RoleId = int.Parse(result.Claims.First(x => x.Type == "roleId").Value),
                SessionId = Guid.Parse(result.Claims.First(x => x.Type == "sessionId").Value)
            };

            return payload;
        }
    }
}
