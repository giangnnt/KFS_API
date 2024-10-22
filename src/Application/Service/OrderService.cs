using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.VNPay;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace KFS.src.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IVNPayService _vNPayService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWalletRepository _walletRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly IBatchRepository _batchRepository;

        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, IUserRepository userRepository, ICartRepository cartRepository, IVNPayService vNPayService, IHttpContextAccessor httpContextAccessor, IProductRepository productRepository, IPaymentRepository paymentRepository, IWalletRepository walletRepository, IPromotionRepository promotionRepository, IBatchRepository batchRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _vNPayService = vNPayService;
            _httpContextAccessor = httpContextAccessor;
            _paymentRepository = paymentRepository;
            _productRepository = productRepository;
            _walletRepository = walletRepository;
            _promotionRepository = promotionRepository;
            _batchRepository = batchRepository;
        }

        public async Task<ResponseDto> CreateOrderFromCart(OrderCreateFromCart req)
        {
            var response = new ResponseDto();
            try
            {
                //map order create to order
                var mappedOrder = _mapper.Map<Order>(req);

                //check if cart exist
                var cart = await _cartRepository.GetCartById(req.CartId);
                if (cart == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Cart not found";
                    response.IsSuccess = false;
                    return response;
                }
                //check if cart is active
                if (cart.Status != CartStatusEnum.Active)
                {
                    response.StatusCode = 400;
                    response.Message = "Cart is not active";
                    response.IsSuccess = false;
                    return response;
                }
                //check if cart is empty
                if (cart.CartItems.Count() == 0)
                {
                    response.StatusCode = 400;
                    response.Message = "Cart is empty";
                    response.IsSuccess = false;
                    return response;
                }

                //set cart to order
                mappedOrder = _mapper.Map(cart, mappedOrder);

                //set estimated delivery date
                mappedOrder.EstimatedDeliveryDate = "3 days";

                // Discount code
                if (req.DiscountCode != null)
                {
                    var promotions = await _promotionRepository.GetAllPromotions();
                    var targetPromotion = promotions.FirstOrDefault(x => x.DiscountCode == req.DiscountCode && x.IsActive);
                    if (targetPromotion == null)
                    {
                        response.StatusCode = 404;
                        response.Message = "Promotion not found";
                        response.IsSuccess = false;
                        return response;
                    }
                    else
                    {
                        mappedOrder.Discount = targetPromotion.DiscountPercentage;
                    }

                }
                //set shipping fee
                mappedOrder.ShippingFee = 30000;

                // Use point from wallet
                if (req.UsePoint == true)
                {
                    var wallet = await _walletRepository.GetWalletByUserId(cart.UserId);
                    mappedOrder.TotalPrice = cart.TotalPrice + mappedOrder.ShippingFee - (cart.TotalPrice * mappedOrder.Discount / 100) - wallet.Point;
                    await _walletRepository.UsePoint(wallet.Id, wallet.Point);
                }
                else
                {
                    //calculate total price
                    mappedOrder.TotalPrice = cart.TotalPrice + mappedOrder.ShippingFee - (cart.TotalPrice * mappedOrder.Discount / 100);
                }



                //set order status
                mappedOrder.Status = OrderStatusEnum.Processing;

                //set id order 
                mappedOrder.Id = Guid.NewGuid();

                string data = String.Empty;

                //check payment method
                if (req.PaymentMethod == PaymentMethodEnum.COD)
                {
                    data = "COD";
                }
                if (req.PaymentMethod == PaymentMethodEnum.VNPAY)
                {
                    data = GeneratePaymentUrl(mappedOrder);
                }

                //create order
                var result = await _orderRepository.CreateOrder(mappedOrder);

                //check result
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Order created successfully";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = data
                    };
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Order creation failed";
                    response.IsSuccess = false;
                    response.Result = new ResultDto
                    {
                        Data = data
                    };
                }
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public string GeneratePaymentUrl(Order order)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    return "HttpContext is null";
                }
                return _vNPayService.CreatePaymentUrl(httpContext, new VNPayRequestModel
                {
                    Amount = (double)order.TotalPrice,
                    CreateDate = DateTime.Now,
                    Description = "Payment for order " + order.Id.ToString(),
                    FullName = order.ContactName,
                    OrderId = order.Id
                }, "order");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<ResponseDto> GetResponsePaymentUrl()
        {
            var response = new ResponseDto();
            try
            {
                //check http context 
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "HttpContext is null";
                    response.IsSuccess = false;
                    return response;
                }
                //get query collection
                var collection = httpContext.Request.Query;
                //get response from vnpay
                var vnPayResponseModel = _vNPayService.GetResponse(collection);
                //check response
                if (vnPayResponseModel.Success == false || vnPayResponseModel.VnPayResponseCode != "00" || vnPayResponseModel.TransactionStatus != "00")
                {
                    var payment = new PaymentOrder
                    {
                        Id = Guid.NewGuid(),
                        Amount = decimal.Parse(vnPayResponseModel.Amount) / 100,
                        CreatedAt = DateTime.Now,
                        Currency = "VND",
                        OrderId = Guid.Parse(vnPayResponseModel.OrderId),
                        PaymentMethod = PaymentMethodEnum.VNPAY,
                        Status = PaymentStatusEnum.Failed,
                        TransactionId = vnPayResponseModel.PaymentId,
                        UserId = _orderRepository.GetOrderById(Guid.Parse(vnPayResponseModel.OrderId)).Result.UserId
                    };
                    var result = await _paymentRepository.CreatePayment(payment);
                    if (result)
                    {
                        response.StatusCode = 400;
                        response.Message = "Payment created successfully" + vnPayResponseModel.VnPayResponseCode;
                        response.IsSuccess = false;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Payment creation failed" + vnPayResponseModel.VnPayResponseCode;
                        response.IsSuccess = false;
                        return response;
                    }
                }
                else
                {
                    //create payment
                    var payment = new PaymentOrder
                    {
                        Id = Guid.NewGuid(),
                        Amount = decimal.Parse(vnPayResponseModel.Amount) / 100,
                        CreatedAt = DateTime.Now,
                        Currency = "VND",
                        OrderId = Guid.Parse(vnPayResponseModel.OrderId),
                        PaymentMethod = PaymentMethodEnum.VNPAY,
                        Status = PaymentStatusEnum.Completed,
                        TransactionId = vnPayResponseModel.PaymentId,
                        PaymentType = "Order",
                        UserId = _orderRepository.GetOrderById(Guid.Parse(vnPayResponseModel.OrderId)).Result.UserId
                    };
                    //get and update cart, order
                    var carts = await _cartRepository.GetCartByUserId(payment.UserId);
                    var cart = new Cart();
                    var order = await _orderRepository.GetOrderById(payment.OrderId);
                    var wallet = await _walletRepository.GetWalletByUserId(payment.UserId);
                    //create payment
                    var result = await _paymentRepository.CreatePayment(payment);
                    if (result)
                    {
                        await _walletRepository.AddPoint(wallet.Id, (int)payment.Amount * 5 / 100);
                        foreach (var c in carts)
                        {
                            if (c.Status == CartStatusEnum.Active)
                            {
                                c.Status = CartStatusEnum.Completed;
                                cart = c;
                            }
                        }
                        foreach (var orderItem in order.OrderItems)
                        {
                            var product = await _productRepository.GetProductById(orderItem.ProductId);
                            product.Inventory -= orderItem.Quantity;
                            await _productRepository.UpdateProduct(product);
                        }
                        order.Status = OrderStatusEnum.Paid;
                        response.StatusCode = 200;
                        response.Message = "Payment created successfully";
                        response.IsSuccess = true;
                    }
                    else
                    {
                        foreach (var c in carts)
                        {
                            if (cart.Status == CartStatusEnum.Active)
                            {
                                c.Status = CartStatusEnum.Deactive;
                                cart = c;
                            }
                        }
                        order.Status = OrderStatusEnum.Failed;
                        response.StatusCode = 400;
                        response.Message = "Payment creation failed";
                        response.IsSuccess = false;
                    }
                    await _cartRepository.UpdateCart(cart);
                    await _orderRepository.UpdateOrder(order);
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

        public async Task<ResponseDto> DeleteOrder(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                //get order
                var order = await _orderRepository.GetOrderById(id);
                if (order == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                    return response;
                }

                //delete order
                var result = await _orderRepository.DeleteOrder(id);

                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Order deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Order deletion failed";
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

        public async Task<ResponseDto> GetOrderById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var order = await _orderRepository.GetOrderById(id);
                if (order == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                    return response;
                }

                var mappedOrder = _mapper.Map<OrderDto>(order);
                //map by format
                await GetCartFormat(mappedOrder);

                response.StatusCode = 200;
                response.Message = "Order found";
                response.Result = new ResultDto
                {
                    Data = mappedOrder
                };
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ResponseDto> GetOrderByUserId(Guid userId)
        {
            var response = new ResponseDto();
            try
            {
                var orders = await _orderRepository.GetOrderByUserId(userId);
                var mappedOrders = _mapper.Map<List<OrderDto>>(orders);
                //map by format
                await GetOrdersFormat(mappedOrders);

                if (orders != null && orders.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Orders found";
                    response.Result = new ResultDto
                    {
                        Data = mappedOrders
                    };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Orders not found";
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
        public async Task GetCartFormat(OrderDto mappedOrder)
        {
            foreach (var orderItem in mappedOrder.OrderItems)
            {
                if (orderItem.IsBatch == true)
                {
                    var batch = await _batchRepository.GetBatchById(orderItem.BatchId);
                    orderItem.Batch = _mapper.Map<BatchDto>(batch);
                }
                else
                {
                    var product = await _productRepository.GetProductById(orderItem.ProductId);
                    orderItem.Product = _mapper.Map<ProductDto>(product);
                }
            }
        }
        public async Task GetOrdersFormat(List<OrderDto> mappedOrders)
        {
            foreach (var order in mappedOrders)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    if (orderItem.IsBatch == true)
                    {
                        var batch = await _batchRepository.GetBatchById(orderItem.BatchId);
                        orderItem.Batch = _mapper.Map<BatchDto>(batch);
                    }
                    else
                    {
                        var product = await _productRepository.GetProductById(orderItem.ProductId);
                        orderItem.Product = _mapper.Map<ProductDto>(product);
                    }
                }
            }
        }

        public async Task<ResponseDto> GetOrders()
        {
            var response = new ResponseDto();
            try
            {
                var orders = await _orderRepository.GetOrders();
                var mappedOrders = _mapper.Map<List<OrderDto>>(orders);
                //map by format
                await GetOrdersFormat(mappedOrders);

                if (orders != null && orders.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Orders found";
                    response.Result = new ResultDto
                    {
                        Data = mappedOrders
                    };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Orders not found";
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

        public async Task<ResponseDto> UpdateOrder(OrderUpdate req, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                //get order
                var order = await _orderRepository.GetOrderById(id);
                if (order == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                    return response;
                }
                // check order status
                if (order.Status == OrderStatusEnum.Delivering || order.Status == OrderStatusEnum.Delivered || order.Status == OrderStatusEnum.Completed || order.Status == OrderStatusEnum.Canceled || order.Status == OrderStatusEnum.Failed)
                {
                    response.StatusCode = 400;
                    response.Message = "Can not update order";
                    response.IsSuccess = false;
                    return response;
                }

                //map order update to order
                var mappedOrder = _mapper.Map(req, order);

                //update order
                var result = await _orderRepository.UpdateOrder(mappedOrder);

                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Order updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Order update failed";
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

        public async Task<ResponseDto> UpdateOrderStatus(OrderUpdateStatus req)
        {
            var response = new ResponseDto();
            try
            {
                //get order
                var order = _orderRepository.GetOrderById(req.Id).Result;
                if (order == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                    return response;
                }

                //set order status
                order.Status = req.Status;

                //update order
                var result = await _orderRepository.UpdateOrder(order);

                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Order status updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Order status update failed";
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

        public Task<ResponseDto> CreateOrderOffline(OrderCreateOffline req)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> GetOwnOrder()
        {
            var response = new ResponseDto();
            try
            {
                var HttpContext = _httpContextAccessor.HttpContext;
                if (HttpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "HttpContext is null";
                    response.IsSuccess = false;
                    return response;
                }
                var payload = HttpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Payload is null";
                    response.IsSuccess = false;
                    return response;
                }
                var userId = payload.UserId;
                var orders = await _orderRepository.GetOrderByUserId(userId);
                var mappedOrders = _mapper.Map<List<OrderDto>>(orders);
                //map by format
                await GetOrdersFormat(mappedOrders);

                if (orders != null && orders.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Orders found";
                    response.Result = new ResultDto
                    {
                        Data = mappedOrders
                    };
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Orders not found";
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