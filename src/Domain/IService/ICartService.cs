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
        Task<ResponseDto> AddProductToCart(Guid id, Guid productId);
        Task<ResponseDto> RemoveItemCart(Guid id, Guid ItemId);
        Task<ResponseDto> AddBatchToCart(Guid id, Guid batchId);
        Task<ResponseDto> GetCartByUserId(Guid userId);
        Task<ResponseDto> GetOwnCart();
    }
}