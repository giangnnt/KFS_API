using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using Microsoft.IdentityModel.Tokens;

namespace KFS.src.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, IUserRepository userRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
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

                //check payment method
                if (req.PaymentMethod == PaymentMethodEnum.COD)
                {
                    mappedOrder.ShippingFee = 30;
                }
                if (req.PaymentMethod == PaymentMethodEnum.VNPAY)
                {
                    mappedOrder.ShippingFee = 0;
                }

                //set estimated delivery date
                mappedOrder.EstimatedDeliveryDate = "3 days";

                //calculate total price
                mappedOrder.TotalPrice = cart.TotalPrice + mappedOrder.ShippingFee - ((decimal)req.Discount / 100 * cart.TotalPrice);

                //set order status
                mappedOrder.Status = OrderStatusEnum.Pending;

                //create order
                var result = await _orderRepository.CreateOrder(mappedOrder);

                //set cart status to completed
                cart.Status = CartStatusEnum.Completed;
                var result1 = await _cartRepository.UpdateCart(cart);
                if (!result1)
                {
                    response.StatusCode = 400;
                    response.Message = "Cart status update failed";
                    response.IsSuccess = false;
                    return response;
                }

                //check result
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Order created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Order creation failed";
                    response.IsSuccess = false;
                    return response;
                }
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