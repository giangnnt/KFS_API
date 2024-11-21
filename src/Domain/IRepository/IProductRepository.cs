using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Domain.Entities;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Domain.IRepository
{
    public interface IProductRepository
    {
        public Task<ObjectPaging<Product>> GetProducts(ProductQuery productQuery);
        public Task<ObjectPaging<Product>> GetProductsAdmin(ProductAdminQuery productQuery);
        public Task<Product> GetProductById(Guid id);
        public Task<bool> CreateProduct(Product product);
        public Task<bool> UpdateProduct(Product product);
        public Task<bool> DeleteProduct(Guid id);
        public Task<IEnumerable<Product>> GetProductsByBatchId(Guid id);
    }
}