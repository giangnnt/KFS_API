using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastructure.Context;

namespace KFS.src.infrastructure.Repository
{
    public class MediaRepository : IMediaRepository
    {
        private readonly KFSContext _context;
        public MediaRepository(KFSContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateMedia(Media media)
        {
            media.CreatedAt = DateTime.Now;
            _context.Medias.Add(media);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteMedia(Media media)
        {
            _context.Medias.Remove(media);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Media>> GetAllMediaByProductId(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId) ?? throw new Exception("Product not found");
            return product.Medias;
        }

        public async Task<Media> GetMediaById(Guid mediaId)
        {
            return await _context.Medias.FindAsync(mediaId) ?? throw new Exception("Media not found");
        }

        public async Task<bool> UpdateMediaProduct(Product product, List<Media> medias)
        {
            // get and remove media
            for (int i = 0; i < product.Medias.Count; i++)
            {
                if (!medias.Contains(product.Medias[i]))
                {
                    product.Medias.RemoveAt(i);
                }
            }
            // get and add media
            foreach (var item in medias)
            {
                if (!product.Medias.Contains(item))
                {
                    product.Medias.Add(item);
                }
            }
            _context.Products.Update(product);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}