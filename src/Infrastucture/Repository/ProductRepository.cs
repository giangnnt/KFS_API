using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Infrastucture.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly KFSContext _context;
        public ProductRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<ObjectPaging<Product>> GetProducts(ProductQuery productQuery)
        {
            var query = _context.Products.AsQueryable();
            // search syntax
            query = query.Where(p => EF.Functions.Like(p.Name, $"%{productQuery.Name}%") || string.IsNullOrEmpty(productQuery.Name));
            query = query.Where(p => EF.Functions.Like(p.Origin, $"%{productQuery.Origin}%") || string.IsNullOrEmpty(productQuery.Origin));
            query = query.Where(p => EF.Functions.Like(p.Species, $"%{productQuery.Species}%") || string.IsNullOrEmpty(productQuery.Species));
            query = query.Where(p => (p.Price >= productQuery.PriceStart && p.Price <= productQuery.PriceEnd) || (productQuery.PriceStart == 0 && productQuery.PriceEnd == 0));
            //set total 
            var total = await query.CountAsync();

            // return
            var productList = await query
            .Include(x => x.Category)
            .Include(x => x.Promotions)
            .Include(x => x.Batches)
            .Include(x => x.Medias)
            .Skip((productQuery.Page - 1) * productQuery.PageSize)
            .Take(productQuery.PageSize)
            .ToListAsync();
            return new ObjectPaging<Product>
            {
                List = productList,
                Total = total
            };
        }
        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Promotions)
            .Include(x => x.Batches)
            .Include(x => x.Medias)
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
            if (product == null) throw new Exception("Product not found");
            _context.Products.Remove(product);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public Task<IEnumerable<Product>> GetProductsAdmin(ProductAdminQuery productQuery)
        {
            throw new NotImplementedException();
        }
    }
}