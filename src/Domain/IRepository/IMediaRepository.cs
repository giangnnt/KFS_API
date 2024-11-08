using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface IMediaRepository
    {
        Task<IEnumerable<Media>> GetAllMediaByProductId(Guid productId);
        Task<bool> UpdateMediaProduct(Product product, List<Media> medias);
        Task<bool> CreateMedia(Media media);
        Task<bool> DeleteMedia(Media media);
        Task<Media> GetMediaById(Guid mediaId);
    }
}