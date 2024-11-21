using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KFS.src.infrastructure.Repository
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
            .ThenInclude(x => x.Medias)
            .Include(x => x.Products)
            .ThenInclude(x => x.Feedbacks)
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
            .ThenInclude(x => x.Medias)
            .Include(x => x.Products)
            .ThenInclude(x => x.Feedbacks)
            .Include(x => x.Promotions)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new Exception("Category not found");
        }

        public async Task<bool> UpdateProductCategory(Category category, List<Product> products)
        {
            // clear old product
            category.Products.Clear();
            // add new product
            category.Products = products;
            _context.Categories.Update(category);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}