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
            .Include(x => x.Promotions)
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

        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _context.Categories
            .Include(x => x.Products)
            .Include(x => x.Promotions)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Category not found");
        }

        public async Task<bool> UpdateProductCategory(Category category, List<Product> products)
        {
            // get and remove product
            for (int i = 0; i < products.Count; i++)
            {
                if (!products.Contains(category.Products[i]))
                {
                    category.Products.RemoveAt(i);
                }
            }
            // add new product
            foreach (var item in products)
            {
                if (!category.Products.Contains(item))
                {
                    category.Products.Add(item);
                }
            }
            _context.Categories.Update(category);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}