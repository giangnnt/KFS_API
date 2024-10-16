using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Domain.Entities;
using Microsoft.Identity.Client;

namespace KFS.src.Domain.IRepository
{
    public interface IMediaRepository
    {
        public Task<List<Media>> GetMedias();
        public Task<bool> Create(Media media);
        public Task<bool> Update(Media media);
        public Task<bool> Delete(Guid id);
        public Task<Media> GetMediaById(Guid id);
      


    }
}
