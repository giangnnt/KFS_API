using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KFS.src.Infrastucture.Cache
{
    public interface ICacheService
    {
        Task<T?> Get<T>(string key);
        Task Set<T>(string key, T value);
        Task Set<T>(string key, T value, TimeSpan expiration);
        Task<bool> Update<T>(string key, T value);
        Task Remove(string key);
        Task<bool> Exists(string key);
        Task Clear();
        Task ClearWithPattern(string pattern);
        Task ForceLogout(Guid userId);
    }
}