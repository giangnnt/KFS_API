using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastucture.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.Infrastucture.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly KFSContext _context;
        public CategoryRepository(KFSContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories
            .Include(x => x.Products)
            .ToListAsync();

        }

       
        public async Task<bool> CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null) throw new Exception("Category not found");
            _context.Categories.Remove(category);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return await _context.Categories
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Name == name) ?? throw new Exception("Category not found");
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _context.Categories
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Category not found");
        }

        public async Task<List<Product>> GetProductBy(Guid categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
          var product =await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) throw new Exception("product not found");
            _context.Products.Remove(product);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}