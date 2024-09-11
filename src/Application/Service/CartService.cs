using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.CartDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository, ICartItemRepository cartItemRepository,IUserRepository userRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _cartItemRepository = cartItemRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public Task<ResponseDto> AddProductToCart(Guid cartId, Guid productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> CreateCart(CartCreate req)
        {
            var response = new ResponseDto();
            try
            {
                //map cart
                var mappedCart = _mapper.Map<Cart>(req);
                //check if user id is empty
                if (req.UserId == Guid.Empty)
                {
                    response.StatusCode = 400;
                    response.Message = "User Id is required";
                    response.IsSuccess = false;
                    return response;
                }
                var User =  await _userRepository.GetUserById(req.UserId);
                //map user
                if (User != null)
                {
                    mappedCart.User = User;
                }
                var result = await _cartRepository.CreateCart(mappedCart);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Cart created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Cart creation failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }

        public Task<ResponseDto> DeleteCart(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetCartById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetCarts()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> RemoveProductFromCart(Guid cartId, Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdateCart(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}