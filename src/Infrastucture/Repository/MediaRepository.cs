using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastucture.Repository
{
    public class MediaRepository : IMediaRepository
    {
        private readonly KFSContext _context;
        public MediaRepository(KFSContext context)
        {
            _context = context;
        }
        

        public async Task<bool> Delete(Guid id)
        {
           
            var media = await _context.Medias.FirstOrDefaultAsync(x => x.Id == id);
            if (media == null) throw new Exception("Media not found");
            _context.Medias.Remove(media);
            int result=await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Media>> GetAllMedia()
        {

           return await _context.Medias
                .Include(x=>x.Product)
                .AsNoTracking()
                
                .ToListAsync();
        }

      


        public async Task<bool> UpdateMedia(Guid id)
        {
            var product = await _context.Medias.FirstOrDefaultAsync(x=>x.Id == id);
            if (product == null) throw new Exception("Media not found");
            _context.Medias.Update(product);
            int result=await _context.SaveChangesAsync();
            return result > 0;

        }

        public async Task<bool> CreateMedia(Media media)
        {
           await _context.Medias.AddAsync(media);
            int result= await _context.SaveChangesAsync();
            return result > 0;
        }

      

      
    

        public async Task<Media> GetMediaById(Guid id)
        {
            return await _context.Medias.FirstOrDefaultAsync(x => x.Id == id) ?? throw new("media not found");
        }

        public async Task<Product> GetProductByMediaId(Guid id)
        {
            return await _context.Products.Include(x => x.Medias).FirstOrDefaultAsync(x => x.Id == id) ?? throw new("product not found");
        }

        public async Task<bool> UpdateMedia(Media media)
        {
          
            _context.Medias.Update(media);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
       

    }
}
