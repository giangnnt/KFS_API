namespace KFS.src.Application.Core.Crypto
{
    public interface ICrypto
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string password, string hash);
    }
    public class Crypto : ICrypto
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