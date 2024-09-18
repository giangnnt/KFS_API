using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.OrderDtos;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, IUserRepository userRepository, ICartRepository cartRepository, IVNPayService vNPayService, IHttpContextAccessor httpContextAccessor, IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _vNPayService = vNPayService;
            _httpContextAccessor = httpContextAccessor;
            _paymentRepository = paymentRepository;
        }

        public async Task<ResponseDto> AcceptOrder(Guid id)
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

                //check order status
                if (order.Status != OrderStatusEnum.Pending)
                {
                    response.StatusCode = 400;
                    response.Message = "Order is not pending";
                    response.IsSuccess = false;
                    return response;
                }

                //set order status to accepted
                order.Status = OrderStatusEnum.Processing;

                //update order
                var result = await _orderRepository.UpdateOrder(order);

                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Order accepted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Order acceptance failed";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
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

                //set cart to order
                mappedOrder = _mapper.Map(cart, mappedOrder);

                //set estimated delivery date
                mappedOrder.EstimatedDeliveryDate = "3 days";

                //set shipping fee
                mappedOrder.ShippingFee = 30000;

                //calculate total price
                mappedOrder.TotalPrice = cart.TotalPrice + mappedOrder.ShippingFee - ((decimal)req.Discount / 100 * cart.TotalPrice);

                //set order status
                mappedOrder.Status = OrderStatusEnum.Pending;

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
            catch
            {
                throw;
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
                    Description = "Payment for order" + order.Id.ToString(),
                    FullName = order.ContactName,
                    OrderId = order.Id
                });
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
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "HttpContext is null";
                    response.IsSuccess = false;
                    return response;
                }
                var collection = httpContext.Request.Query;
                var vnPayResponseModel = _vNPayService.GetResponse(collection);
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    Amount = decimal.Parse(vnPayResponseModel.Amount),
                    CreatedAt = DateTime.Now,
                    Currency = "VND",
                    OrderId = Guid.Parse(vnPayResponseModel.OrderId),
                    PaymentMethod = PaymentMethodEnum.VNPAY,
                    Status = PaymentStatusEnum.Completed,
                    TransactionId = vnPayResponseModel.PaymentId,
                    UserId = _orderRepository.GetOrderById(Guid.Parse(vnPayResponseModel.OrderId)).Result.UserId
                };
                var result = await _paymentRepository.CreatePayment(payment);
                var carts = await _cartRepository.GetCartByUserId(payment.UserId);
                var cart =  new Cart();
                var order = await _orderRepository.GetOrderById(payment.OrderId);
                if (result)
                {
                    foreach (var c in carts)
                    {
                        if(c.Status == CartStatusEnum.Active)
                        {
                            c.Status = CartStatusEnum.Completed;
                            cart = c;
                        }
                    }
                    order.Status = OrderStatusEnum.Completed;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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

                response.StatusCode = 200;
                response.Message = "Order found";
                response.Result = new ResultDto
                {
                    Data = mappedOrder
                };
                response.IsSuccess = true;
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> GetOrderByUserId(Guid userId)
        {
            var response = new ResponseDto();
            try
            {
                var orders = await _orderRepository.GetOrderByUserId(userId);
                var mappedOrders = _mapper.Map<List<OrderDto>>(orders);

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
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> GetOrders()
        {
            var response = new ResponseDto();
            try
            {
                var orders = await _orderRepository.GetOrders();
                var mappedOrders = _mapper.Map<List<OrderDto>>(orders);

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
            catch
            {
                throw;
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

                //map order update to order
                var mappedOrder = _mapper.Map(req, order);

                //calculate total price


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
            catch
            {
                throw;
            }
        }
    }
}