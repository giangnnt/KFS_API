using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IService
{
    public interface ICategoryService
    {
        Task<ResponseDto> GetCategories();
        Task<ResponseDto> CreateCategory(CategoryCreate category);
        Task<ResponseDto> UpdateCategory(CategoryUpdate category, Guid id);
        Task<ResponseDto> DeleteCategory(Guid id);
        Task<ResponseDto> GetCategoryById(Guid id);
        Task<ResponseDto> GetProductByCategory(Guid id);
        Task<ResponseDto> UpdateProductCategory(Guid categoryId, List<Guid> productId);
    }
}