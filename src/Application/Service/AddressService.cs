using System.Runtime.InteropServices;
using AutoMapper;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.AddressDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{

    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOwnerService _ownerService;
        public AddressService(IAddressRepository addressRepository, IMapper mapper, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IOwnerService ownerService)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _ownerService = ownerService;
        }
        public async Task<ResponseDto> CreateAddress(AddressCreate addressCreate)
        {
            var response = new ResponseDto();
            try
            {
                // get http context
                var httpContext = _httpContextAccessor.HttpContext;
                // check http context
                if (httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Http context not found";
                    response.IsSuccess = false;
                    return response;
                }
                // get payload
                var payload = httpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Payload not found";
                    response.IsSuccess = false;
                    return response;
                }
                var user = await _userRepository.GetUserById(payload.UserId);
                if (user == null)
                {
                    response.StatusCode = 400;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }
                var mappedAddress = _mapper.Map<Address>(addressCreate);
                mappedAddress.UserId = user.Id;
                var result = await _addressRepository.AddAddressAsync(mappedAddress);
                if (!result)
                {
                    response.StatusCode = 400;
                    response.Message = "Failed to create address";
                    response.IsSuccess = false;
                    return response;
                }
                response.StatusCode = 201;
                response.Message = "Address created successfully";
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> DeleteAddress(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var result = await _addressRepository.DeleteAddressAsync(id);
                if (!result)
                {
                    response.StatusCode = 400;
                    response.Message = "Failed to delete address";
                    response.IsSuccess = false;
                    return response;
                }
                response.StatusCode = 200;
                response.Message = "Address deleted successfully";
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetAddressById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var address = await _addressRepository.GetAddressById(id);
                if (address == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Address not found";
                    response.IsSuccess = false;
                    return response;
                }
                var mappedAddress = _mapper.Map<AddressDto>(address);
                response.StatusCode = 200;
                response.Message = "Address retrieved successfully";
                response.Result = new ResultDto
                {
                    Data = mappedAddress
                };
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetAddressByUserId(Guid userId)
        {
            var response = new ResponseDto();
            try
            {
                var addresses = await _addressRepository.GetAddressByUserId(userId);
                if (addresses == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Address not found";
                    response.IsSuccess = false;
                    return response;
                }
                var mappedAddresses = _mapper.Map<List<AddressDto>>(addresses);
                response.StatusCode = 200;
                response.Message = "Address retrieved successfully";
                response.Result = new ResultDto
                {
                    Data = mappedAddresses
                };
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;

            }
        }

        public async Task<ResponseDto> GetAddressOwn()
        {
            var response = new ResponseDto();
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Http context not found";
                    response.IsSuccess = false;
                    return response;
                }
                var payload = httpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Payload not found";
                    response.IsSuccess = false;
                    return response;
                }

                var address = await _addressRepository.GetAddressByUserId(payload.UserId);
                if (address == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Address not found";
                    response.IsSuccess = false;
                    return response;
                }
                var mappedAddress = _mapper.Map<List<AddressDto>>(address);
                response.StatusCode = 200;
                response.Message = "Address retrieved successfully";
                response.Result = new ResultDto
                {
                    Data = mappedAddress
                };
                response.IsSuccess = true;
                return response;
            }
            catch (Exception e)
            {
                response.StatusCode = 500;
                response.Message = e.Message;
                response.IsSuccess = false;
                return response;
            }
        }
    }
}