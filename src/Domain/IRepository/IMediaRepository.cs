using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Domain.Entities;
using Microsoft.Identity.Client;

namespace KFS.src.Domain.IRepository
{
    public interface IMediaRepository
    {
        public Task<List<Media>> GetAllMedia();
        public Task<bool> CreateMedia(Media media);
        public Task<bool> UpdateMedia(Media media);
        public Task<bool> Delete(Guid id);
        public Task<Media> GetMediaById(Guid id);
        public Task<Product> GetProductByMediaId(Guid id);


    }
}
