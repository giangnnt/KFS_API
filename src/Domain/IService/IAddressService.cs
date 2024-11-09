using KFS.src.Application.Dto.AddressDtos;
using KFS.src.Application.Dto.ResponseDtos;

namespace KFS.src.Domain.IService
{
    public interface IAddressService
    {
        Task<ResponseDto> CreateAddress(AddressCreate addressCreate);
        //Task<ResponseDto> UpdateAddress(AddressUpdate addressUpdate);
        Task<ResponseDto> DeleteAddress(Guid id);
        Task<ResponseDto> GetAddressById(Guid id);
        Task<ResponseDto> GetAddressByUserId(Guid userId);
        Task<ResponseDto> GetAddressOwn();
    }
}