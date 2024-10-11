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
        Task<ResponseDto> GetAllCategories();
        Task<ResponseDto> GetCategoryByName(string name);
        Task<ResponseDto> CreateCategory(categorydtov2 category);
        Task<ResponseDto> UpdateCategory(categoryv3 category, Guid id);
        Task<ResponseDto> DeleteCategory(Guid id);
        Task<ResponseDto> GetCategoryById(Guid id);
        Task<ResponseDto> GetProductByCateId(Guid id);
        Task <ResponseDto>DeleteProductByProId(Guid id);
    }
}