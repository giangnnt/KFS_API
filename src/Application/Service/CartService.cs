using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using KFS.src.Infrastucture.Context;

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
        public CartService(ICartRepository cartRepository, IProductRepository productRepository, IUserRepository userRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IBatchRepository batchRepository, ICartItemRepository cartItemRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _batchRepository = batchRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<ResponseDto> AddBatchToCart(BatchAddRemoveDto req)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartById(req.CartId);
                var batch = await _batchRepository.GetBatchById(req.BatchId);
                var product = await _productRepository.GetProductById(batch.ProductId);
                //check if batch is for sell
                if (batch.IsForSell == false)
                {
                    response.StatusCode = 400;
                    response.Message = "Batch is not for sell";
                    response.IsSuccess = false;
                    return response;
                }
                //check if any cart item exists rules: same product, is a batch, same quantity
                var cartItem = cart.CartItems.Where(x => x.ProductId == batch.ProductId && x.IsBatch == true && x.Quantity == batch.Quantity).FirstOrDefault();
                if (cartItem == null)
                {
                    // check if any cart item is a same product but not a batch
                    var cartItem2 = cart.CartItems.Where(x => x.ProductId == batch.ProductId && x.IsBatch == false);
                    if (cartItem2 != null && cartItem2.Count() > 0)
                    {
                        response.StatusCode = 400;
                        response.Message = "You can only choose one out of two: product or cart";
                        response.IsSuccess = false;
                        return response;
                    }
                    //check inventory
                    if (batch.Inventory < req.Quantity)
                    {
                        response.StatusCode = 400;
                        response.Message = "Batch quantity is not enough";
                        response.IsSuccess = false;
                        return response;
                    }
                    //create new cart item
                    var cartItemToAdd = new CartItem
                    {
                        CartId = req.CartId,
                        ProductId = batch.ProductId,
                        BatchId = batch.Id,
                        Quantity = req.Quantity,
                        Price = batch.Price * req.Quantity,
                        IsBatch = true,
                        Product = product
                    };
                    cart.CartItems.Add(cartItemToAdd);
                }
                else
                {
                    //check inventory
                    if (batch.Inventory < cartItem.Quantity + req.Quantity)
                    {
                        response.StatusCode = 400;
                        response.Message = "Batch quantity is not enough";
                        response.IsSuccess = false;
                        return response;
                    }
                    //update cart item
                    cartItem.Quantity += req.Quantity;
                    cartItem.Price = batch.Price * cartItem.Quantity;
                }
                // update cart
                cart.TotalItem += req.Quantity;
                cart.TotalPrice += batch.Price * req.Quantity;
                var result = await _cartRepository.AddRemoveCartItem(cart);
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


        public async Task<ResponseDto> AddProductToCart(CartAddRemoveDto req)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartById(req.CartId);
                var product = await _productRepository.GetProductById(req.ProductId);
                //check if product is for sell
                if (product.IsForSell == false)
                {
                    response.StatusCode = 400;
                    response.Message = "Product is not for sell";
                    response.IsSuccess = false;
                    return response;
                }

                //check if any cart item exists
                var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == req.ProductId && x.IsBatch == false);
                if (cartItem == null)
                {
                    var cartItem2 = cart.CartItems.Where(x => x.ProductId == req.ProductId && x.IsBatch == true);
                    if (cartItem2 != null && cartItem2.Count() > 0)
                    {
                        response.StatusCode = 400;
                        response.Message = "You can only choose one out of two: product or cart";
                        response.IsSuccess = false;
                        return response;
                    }
                    //check inventory
                    if (product.Inventory < req.Quantity)
                    {
                        response.StatusCode = 400;
                        response.Message = "Product quantity is not enough";
                        response.IsSuccess = false;
                        return response;
                    }
                    //create new cart item
                    cartItem = new CartItem
                    {
                        CartId = req.CartId,
                        ProductId = req.ProductId,
                        Quantity = req.Quantity,
                        Price = product.Price * req.Quantity,
                        Product = product
                    };
                    cart.CartItems.Add(cartItem);
                }
                else
                {
                    //check inventory
                    if (product.Inventory < cartItem.Quantity + req.Quantity)
                    {
                        response.StatusCode = 400;
                        response.Message = "Product quantity is not enough";
                        response.IsSuccess = false;
                        return response;
                    }
                    //update cart item
                    cartItem.Quantity += req.Quantity;
                    cartItem.Price = product.Price * cartItem.Quantity;
                }
                //update cart
                cart.TotalItem += req.Quantity;
                cart.TotalPrice += product.Price * req.Quantity;
                var result = await _cartRepository.AddRemoveCartItem(cart);
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
                //map by format
                await GetCartFormat(mappedCart);

                if (cart != null)
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
        public async Task GetCartFormat(CartDto mappedCart)
        {
            foreach (var cartItem in mappedCart.CartItems)
            {
                if (cartItem.IsBatch == true)
                {
                    var batch = await _batchRepository.GetBatchById(cartItem.BatchId);
                    cartItem.Batch = _mapper.Map<BatchDto>(batch);
                }
                else
                {
                    var product = await _productRepository.GetProductById(cartItem.ProductId);
                    cartItem.Product = _mapper.Map<ProductDto>(product);
                }
            }
        }
        public async Task GetCartsFormat(List<CartDto> mappedCarts)
        {
            foreach (var cart in mappedCarts)
            {
                foreach (var cartItem in cart.CartItems)
                {
                    if (cartItem.IsBatch == true)
                    {
                        var batch = await _batchRepository.GetBatchById(cartItem.BatchId);
                        cartItem.Batch = _mapper.Map<BatchDto>(batch);
                    }
                    else
                    {
                        var product = await _productRepository.GetProductById(cartItem.ProductId);
                        cartItem.Product = _mapper.Map<ProductDto>(product);
                    }
                }
            }
        }

        public async Task<ResponseDto> GetCarts()
        {
            var response = new ResponseDto();
            try
            {
                var carts = await _cartRepository.GetCarts();
                var mappedCarts = _mapper.Map<List<CartDto>>(carts);
                //map by format
                await GetCartsFormat(mappedCarts);

                if (carts != null && carts.Count() > 0)
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

        public async Task<ResponseDto> RemoveBatchFromCart(BatchAddRemoveDto req)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartById(req.CartId);
                var batch = await _batchRepository.GetBatchById(req.BatchId);
                //check if any cart item exists
                var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == batch.ProductId);
                if (cartItem == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Batch not found in cart";
                    response.IsSuccess = false;
                    return response;
                }
                //check if req quantity is equal to cart item quantity
                if (req.Quantity == cartItem.Quantity)
                {
                    //remove cart item
                    cart.CartItems.Remove(cartItem);
                }
                //check if quantity is greater than cart item quantity
                if (req.Quantity > cartItem.Quantity)
                {
                    response.StatusCode = 400;
                    response.Message = "Quantity is greater than cart item quantity";
                    response.IsSuccess = false;
                    return response;
                }
                //update cart item
                cartItem.Quantity -= req.Quantity;
                cartItem.Price = batch.Price * cartItem.Quantity;
                await _cartItemRepository.UpdateCartItem(cartItem);
                //update cart
                cart.TotalItem -= req.Quantity;
                cart.TotalPrice -= batch.Price * req.Quantity;
                var result = await _cartRepository.UpdateCart(cart);
                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Batch removed from cart successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Batch removal from cart failed";
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

        public async Task<ResponseDto> RemoveProductFromCart(CartAddRemoveDto req)
        {
            var response = new ResponseDto();
            try
            {
                var cart = await _cartRepository.GetCartById(req.CartId);
                var product = await _productRepository.GetProductById(req.ProductId);
                //check if any cart item exists
                var cartItem = cart.CartItems.FirstOrDefault(x => x.ProductId == req.ProductId);
                if (cartItem == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Product not found in cart";
                    response.IsSuccess = false;
                    return response;
                }
                //check if req uantity is equal to cart item quantity
                if (req.Quantity == cartItem.Quantity)
                {
                    //remove cart item
                    cart.CartItems.Remove(cartItem);
                }

                //check if quantity is greater than cart item quantity
                if (req.Quantity > cartItem.Quantity)
                {
                    response.StatusCode = 400;
                    response.Message = "Quantity is greater than cart item quantity";
                    response.IsSuccess = false;
                    return response;
                }
                //update cart item
                cartItem.Quantity -= req.Quantity;
                cartItem.Price = product.Price * cartItem.Quantity;
                await _cartItemRepository.UpdateCartItem(cartItem);
                //update cart
                cart.TotalItem -= req.Quantity;
                cart.TotalPrice -= product.Price * req.Quantity;
                var result = await _cartRepository.UpdateCart(cart);
                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Product removed from cart successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Product removal from cart failed";
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
                var cart = _cartRepository.GetCartByUserId(userId);
                var mappedCart = _mapper.Map<CartDto>(cart);
                //map by format
                await GetCartFormat(mappedCart);
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
                //map by format
                await GetCartsFormat(mappedCarts);
                if (mappedCarts != null)
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