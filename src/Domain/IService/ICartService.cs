using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using KFS.src.Application.Dto.CartDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Domain.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCarts();
        Task<ResponseDto> GetCartById(Guid id);
        Task<ResponseDto> CreateCart(CartCreate req);
        Task<ResponseDto> UpdateCart(CartUpdate req, Guid id);
        Task<ResponseDto> DeleteCart(Guid id);
        Task<ResponseDto> AddProductToCart(CartAddRemoveDto req);
        Task<ResponseDto> RemoveProductFromCart(CartAddRemoveDto req);
        Task<ResponseDto> AddBatchToCart(BatchAddRemoveDto req);
        Task<ResponseDto> RemoveBatchFromCart(BatchAddRemoveDto req);
        Task<ResponseDto> GetCartByUserId(Guid userId);
    }
}