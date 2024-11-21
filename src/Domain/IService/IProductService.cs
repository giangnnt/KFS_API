using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IProductService
    {
        public Task<ResponseDto> GetProducts(ProductQuery productQuery);
        public Task<ResponseDto> GetProductById(Guid id);
        public Task<ResponseDto> CreateProduct(ProductCreate req);
        public Task<ResponseDto> UpdateProduct(ProductUpdate req, Guid id);
        public Task<ResponseDto> DeleteProduct(Guid id);
        public Task<ResponseDto> UpdateIsForSell(bool isForSell, Guid id);
        public Task<ResponseDto> GetProductsAdmin(ProductAdminQuery productQuery);
        public Task<ResponseDto> UpdateProductIsActive(bool isActive, Guid id);
    }
}