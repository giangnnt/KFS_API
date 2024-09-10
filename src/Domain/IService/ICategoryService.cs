using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IService
{
    public interface ICategoryService
    {
        Task<ResponseDto> GetAllCategories();
        Task<ResponseDto> GetCategoryById(int id);
        Task<ResponseDto> CreateCategory(Category category);
        Task<ResponseDto> UpdateCategory(Category category);
        Task<ResponseDto> DeleteCategory(int id);
    }
}