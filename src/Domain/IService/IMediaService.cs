using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IMediaService
    {
        Task<ResponseDto> UploadMedia(IFormFile file, string type);
        Task<ResponseDto> DeleteMedia(Guid mediaId);
        Task<ResponseDto> GetMediaByProductId(Guid productId);
        Task<ResponseDto> UpdateMediaProduct(Guid productId, List<Guid> mediaIds);
        Task<ResponseDto> CreateMedia(MediaCreate mediaCreate);
    }
}