using KFS.src.Application.Dto.CredentialDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface ICredentialService
    {
        Task<ResponseDto> GetCredentials();
        Task<ResponseDto> GetCredentialById(Guid id);
        Task<ResponseDto> CreateCredential(credentialCreate req);
        Task<ResponseDto> UpdateCredential(CredentialUpdate req, Guid id);
        Task<ResponseDto> DeleteCredential(Guid id);
        Task<ResponseDto> GetCredentialsByProductId(Guid productId);
        Task<ResponseDto> GetCredentialsByUserOrderHistory(HttpContext context);
    }
}