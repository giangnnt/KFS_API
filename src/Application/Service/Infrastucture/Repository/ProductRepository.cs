using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastucture.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly KFSContext _context;
        public ProductRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products
            .Include(x => x.Category)
            .Include(x=>x.Promotions)
            .Include(x=>x.Batches)
            .Include(x=>x.Medias)
            .ToListAsync();
        }
        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products
            .Include(x => x.Category)
            .Include(x=>x.Promotions)
            .Include(x=>x.Batches)
            .Include(x=>x.Medias)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Product not found");
        }
        public async Task<bool> CreateProduct(Product product)
        {
            product.CreatedAt = DateTime.Now;
            _context.Products.Add(product);
             int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> UpdateProduct(Product product)
        {
            product.UpdatedAt = DateTime.Now;
            _context.Products.Update(product);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public async Task<bool> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if(product == null) throw new Exception("Product not found");
            _context.Products.Remove(product);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

      
    }
}