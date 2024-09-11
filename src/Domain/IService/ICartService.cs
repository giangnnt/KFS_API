using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCarts();
        Task<ResponseDto> GetCartById(Guid id);
        Task<ResponseDto> CreateCart();
        Task<ResponseDto> UpdateCart(Guid id);
        Task<ResponseDto> DeleteCart(Guid id);
        Task<ResponseDto> AddProductToCart(Guid cartId, Guid productId);
        Task<ResponseDto> RemoveProductFromCart(Guid cartId, Guid productId);
    }
}