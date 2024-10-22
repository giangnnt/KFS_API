using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IRepository
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(Guid id);
        Task<Category> GetCategoryById(Guid id);
        Task<bool> UpdateProductCategory(Category category, List<Product> products);
    }
}