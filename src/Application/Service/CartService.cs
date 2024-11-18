using AutoMapper;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.CartDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBatchRepository _batchRepository;
        private readonly HttpContext _httpContext;
        private readonly IOwnerService _ownerService;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IBatchRepository batchRepository, ICartItemRepository cartItemRepository, IOwnerService ownerService)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _batchRepository = batchRepository;
            _cartItemRepository = cartItemRepository;
            _ownerService = ownerService;
            // http context
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("Http context is required.");

        }

        public async Task<ResponseDto> AddBatchToCart(Guid id, Guid batchId)
        {
            var response = new ResponseDto();
            try
            {
                var batch = await _batchRepository.GetBatchById(batchId);
                var cart = await _cartRepository.GetCartById(id);
                //check if cart is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, cart.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                //check if batch is for sell
                if (batch.IsForSell == false)
                {
                    response.StatusCode = 400;
                    response.Message = "Batch is not for sell";
                    response.IsSuccess = false;
                    return response;
                }
                // check batch status
                if (batch.Status != ProductStatusEnum.Active)
                {
                    response.StatusCode = 400;
                    response.Message = "Can not add to cart";
                    response.IsSuccess = false;
                    return response;
                }
                // add batch to cart
                var batchItem = new CartItemBatch
                {
                    CartId = id,
                    BatchId = batchId,
                    Price = batch.Price,
                    Weight = batch.Weight,
                    Batch = batch
                };
                // update cart
                cart.CartItems.Add(batchItem);
                cart.TotalPrice += batch.Price;
                cart.TotalItem += 1;
                cart.TotalWeight += batch.Weight;
                var result = await _cartRepository.UpdateCart(cart);
                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Batch added to cart successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Batch addition to cart failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> AddProductToCart(Guid id, Guid productId)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartById(id);
                var product = await _productRepository.GetProductById(productId);
                //check if cart is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, cart.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                // check if product is for sell
                if (product.IsForSell == false)
                {
                    response.StatusCode = 400;
                    response.Message = "Product is not for sell";
                    response.IsSuccess = false;
                    return response;
                }
                // check product status
                if (product.Status != ProductStatusEnum.Active && product.Status != ProductStatusEnum.Consignment)
                {
                    response.StatusCode = 400;
                    response.Message = "Can not add to cart";
                    response.IsSuccess = false;
                    return response;
                }
                // add product to cart
                var productItem = new CartItemProduct
                {
                    CartId = id,
                    ProductId = productId,
                    Price = product.Price,
                    Weight = product.Weight,
                    Product = product
                };
                // update cart
                cart.CartItems.Add(productItem);
                cart.TotalPrice += product.Price;
                cart.TotalItem += 1;
                cart.TotalWeight += product.Weight;
                var result = await _cartRepository.UpdateCart(cart);
                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Product added to cart successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Product addition to cart failed";
                    response.IsSuccess = false;
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> CreateCart(CartCreate req)
        {
            var response = new ResponseDto();
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                // check httpContext
                if (httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Http context is required";
                    response.IsSuccess = false;
                    return response;
                }
                // get payload
                var payload = httpContext.Items["payload"] as Payload;
                // check payload
                if (payload == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Payload is required";
                    response.IsSuccess = false;
                    return response;
                }
                // get userId
                var userId = payload.UserId;
                //map cart
                var mappedCart = _mapper.Map<Cart>(req);
                //check if user exist
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    response.StatusCode = 404;
                    response.Message = "User not found";
                    response.IsSuccess = false;
                    return response;
                }

                //map user
                if (user != null)
                {
                    mappedCart.User = user;
                }
                var userCarts = await _cartRepository.GetCartByUserId(userId);
                //check if user has an active cart
                if (req.Status == CartStatusEnum.Active)
                {
                    //check if user has an active cart
                    foreach (var userCart in userCarts)
                    {
                        if (userCart.Status == CartStatusEnum.Active)
                        {
                            response.StatusCode = 400;
                            response.Message = "User already has an active cart";
                            response.IsSuccess = false;
                            return response;
                        }
                    }
                }
                mappedCart.Status = req.Status;
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> DeleteCart(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartById(id);
                //check if cart is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, cart.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var result = await _cartRepository.DeleteCart(id);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Cart deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Cart deletion failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetCartById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartById(id);
                var mappedCart = _mapper.Map<CartDto>(cart);
                if (mappedCart != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Cart found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCart
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Cart not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetCarts()
        {
            var response = new ResponseDto();
            try
            {
                var carts = await _cartRepository.GetCarts();
                var mappedCarts = _mapper.Map<List<CartDto>>(carts);

                if (mappedCarts != null && mappedCarts.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Carts found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCarts
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No carts found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> RemoveItemCart(Guid id, Guid ItemId)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartById(id);
                var Item = await _cartItemRepository.GetCartItemById(ItemId);
                //check if cart is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, cart.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                // update cart
                cart.CartItems.Remove(Item);
                cart.TotalPrice -= Item.Price;
                cart.TotalItem -= 1;
                cart.TotalWeight -= Item.Weight;
                var result = await _cartRepository.UpdateCart(cart);
                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Cart Item removed from cart successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Cart Item removal from cart failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> UpdateCart(CartUpdate req, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartById(id);
                //check if cart is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, cart.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                if (cart == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Cart not found";
                    response.IsSuccess = false;
                    return response;
                }

                if (req.Status == CartStatusEnum.Active)
                {
                    var userCarts = await _cartRepository.GetCarts();
                    //check if user has an active cart
                    foreach (var userCart in userCarts)
                    {
                        if (userCart.Status == CartStatusEnum.Active)
                        {
                            response.StatusCode = 400;
                            response.Message = "User already has an active cart";
                            response.IsSuccess = false;
                            return response;
                        }
                    }
                }

                //map cart
                var mappedCart = _mapper.Map(req, cart);

                //update cart
                var result = await _cartRepository.UpdateCart(mappedCart);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Cart updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Cart update failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetCartByUserId(Guid userId)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartByUserId(userId);
                var mappedCart = _mapper.Map<List<CartDto>>(cart);
                if (mappedCart != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Cart found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCart
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Cart not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetOwnCart()
        {
            var response = new ResponseDto();
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                // check httpContext
                if (httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Http context is required";
                    response.IsSuccess = false;
                    return response;
                }
                // get payload
                var payload = httpContext.Items["payload"] as Payload;
                // check payload
                if (payload == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Payload is required";
                    response.IsSuccess = false;
                    return response;
                }
                // get userId
                var userId = payload.UserId;
                var carts = await _cartRepository.GetCartByUserId(userId);
                var mappedCarts = _mapper.Map<List<CartDto>>(carts);
                if (mappedCarts != null && mappedCarts.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Cart found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedCarts
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Cart not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
    }
}