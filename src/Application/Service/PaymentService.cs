using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

namespace KFS.src.Application.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShipmentRepository _shipmentRepository;
        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IShipmentRepository shipmentRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _shipmentRepository = shipmentRepository;
        }
        public Task<ResponseDto> CreatePayment()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseDto> CreatePaymentByOrderIdCOD(Guid orderId)
        {
            var response = new ResponseDto();
            try
            {
                var order = await _orderRepository.GetOrderById(orderId);
                if (order == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                    return response;
                }
                if (order.Status != OrderStatusEnum.Delivered)
                {
                    response.StatusCode = 400;
                    response.Message = "Order has not been delivered";
                    response.IsSuccess = false;
                    return response;
                }
                if(order.PaymentMethod != PaymentMethodEnum.COD)
                {
                    response.StatusCode = 400;
                    response.Message = "Payment method is not Cash On Delivery";
                    response.IsSuccess = false;
                    return response;
                }
                var payment = new Payment
                {
                    OrderId = orderId,
                    UserId = order.UserId,
                    PaymentMethod = order.PaymentMethod,
                    Amount = order.TotalPrice,
                    Status = PaymentStatusEnum.Completed,
                    CreatedAt = DateTime.Now
                };
                //get and update cart, order
                var carts = await _cartRepository.GetCartByUserId(payment.UserId);
                var cart = new Cart();
                var result = await _paymentRepository.CreatePayment(payment);
                if (result)
                {
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
                        // update product inventory
                        var product = await _productRepository.GetProductById(orderItem.ProductId);
                        product.Inventory -= orderItem.Quantity;
                        await _productRepository.UpdateProduct(product);
                    }
                    order.Status = OrderStatusEnum.Completed;
                    response.StatusCode = 201;
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
                if(order.Shipment != null)
                {
                    order.Shipment.Status = ShipmentStatusEnum.Completed;
                }
                await _shipmentRepository.UpdateShipment(order.Shipment!);
                await _cartRepository.UpdateCart(cart);
                await _orderRepository.UpdateOrder(order);
                return response;
            }
            catch
            {
                throw;
            }
        }

        public Task<ResponseDto> DeletePayment(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetPaymentById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> GetPayments()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto> UpdatePayment()
        {
            throw new NotImplementedException();
        }
    }
}