using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface ICredentialRepositoty
    {
        Task<bool> CreateCredential(Credential credential);
        Task<bool> UpdateCredential(Credential credential);
        Task<bool> DeleteCredential(Guid id);
        Task<Credential> GetCredentialById(Guid id);
        Task<List<Credential>> GetCredentials();
        Task<List<Credential>> GetCredentialsByProductId(Guid productId);
    }
}