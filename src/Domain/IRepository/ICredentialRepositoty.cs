using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface ICredentialRepository
    {
        Task<bool> CreateCredential(Credential credential);
        Task<bool> UpdateCredential(Credential credential);
        Task<bool> DeleteCredential(Guid id);
        Task<Credential> GetCredentialById(Guid id);
        Task<List<Credential>> GetCredentials();
        Task<List<Credential>> GetCredentialsByProductId(Guid productId);
        Task<List<Credential>> GetCredentialsByUserOrderHistory(Guid userId);
    }
}