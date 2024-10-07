using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Application.Core.Crypto
{
    public interface ICrypro
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hash);
    }
    public class Crypto : ICrypro
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}