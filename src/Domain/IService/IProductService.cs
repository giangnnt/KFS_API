using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IService   
{
    public interface IProductService
    {
        public Task<ResponseDto> GetProducts();
        public Task<ResponseDto> GetProductById(Guid id);
        public Task<ResponseDto> CreateProduct(ProductCreate req);
        public Task<ResponseDto> UpdateProduct(ProductUpdate req, Guid id);
        public Task<ResponseDto> DeleteProduct(Guid id);
        public Task<ResponseDto> UpdateIsForSell(bool isForSell, Guid id);
    }
}