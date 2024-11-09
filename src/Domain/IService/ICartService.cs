using KFS.src.Application.Dto.CartDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCarts();
        Task<ResponseDto> GetCartById(Guid id);
        Task<ResponseDto> CreateCart(CartCreate req);
        Task<ResponseDto> UpdateCart(CartUpdate req, Guid id);
        Task<ResponseDto> DeleteCart(Guid id);
        Task<ResponseDto> AddProductToCart(Guid id, CartAddRemoveDto req);
        Task<ResponseDto> RemoveProductFromCart(Guid id, CartAddRemoveDto req);
        Task<ResponseDto> AddBatchToCart(Guid id, BatchAddRemoveDto req);
        Task<ResponseDto> RemoveBatchFromCart(Guid id, BatchAddRemoveDto req);
        Task<ResponseDto> GetCartByUserId(Guid userId);
        Task<ResponseDto> GetOwnCart();
    }
}