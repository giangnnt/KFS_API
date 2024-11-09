using AutoMapper;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }
        public async Task<ResponseDto> CreateCategory(CategoryCreate category)
        {
            var response = new ResponseDto();
            try
            {
                var mappedCategory = _mapper.Map<Category>(category);
                var result = await _categoryRepository.CreateCategory(mappedCategory);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Category created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Category creation failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> DeleteCategory(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _categoryRepository.DeleteCategory(id);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Category deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Category deletion failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetCategories()
        {
            var response = new ResponseDto();
            try
            {
                var categories = await _categoryRepository.GetCategories();
                var mappedCategories = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                if (mappedCategories != null && mappedCategories.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Categories retrieved successfully";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCategories
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No category found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetCategoryById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
                var mappedCategory = _mapper.Map<CategoryDto>(category);
                if (mappedCategory != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Category found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCategory
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Category not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetProductByCategory(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
                var mappedCategory = _mapper.Map<CategoryDto>(category);
                if (mappedCategory != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Category found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCategory
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Category not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> UpdateCategory(CategoryUpdate category, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var categoryToUpdate = await _categoryRepository.GetCategoryById(id);
                var mappedCategory = _mapper.Map(category, categoryToUpdate);
                var result = await _categoryRepository.UpdateCategory(mappedCategory);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Category updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Category update failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> UpdateProductCategory(Guid categoryId, List<Guid> productId)
        {
            var response = new ResponseDto();
            try
            {
                var category = await _categoryRepository.GetCategoryById(categoryId);
                var products = new List<Product>();
                foreach (var id in productId)
                {
                    var product = await _productRepository.GetProductById(id);
                    products.Add(product);
                }
                var result = await _categoryRepository.UpdateProductCategory(category, products);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Product updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Product update failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }



        }
    }
}