using AutoMapper;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.PaymentDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBatchRepository _batchRepository;
        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IShipmentRepository shipmentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IBatchRepository batchRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _shipmentRepository = shipmentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _batchRepository = batchRepository;
        }
        public async Task<ResponseDto> CreatePaymentOffline(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var listCredential = new List<Credential>();
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
                // check order offline
                var order = await _orderRepository.GetOrderById(id);
                if (order.PaymentMethod != PaymentMethodEnum.OFFLINE)
                {
                    response.StatusCode = 400;
                    response.Message = "Payment method is not Offline";
                    response.IsSuccess = false;
                    return response;
                }
                var payment = new PaymentOrder
                {
                    OrderId = id,
                    PaymentType = "Order",
                    UserId = order.UserId,
                    PaymentMethod = order.PaymentMethod,
                    Amount = order.TotalPrice,
                    Status = PaymentStatusEnum.Completed,
                    CreatedAt = DateTime.Now
                };
                var result = await _paymentRepository.CreatePayment(payment);
                foreach (var orderItem in order.OrderItems)
                {
                    // order item product
                    if (!orderItem.IsBatch)
                    {
                        //update inventory
                        var product = await _productRepository.GetProductById(orderItem.ProductId);
                        product.Inventory -= orderItem.Quantity;
                        if (product.Inventory == 0)
                        {
                            product.Status = ProductStatusEnum.SoldOut;
                        }
                        await _productRepository.UpdateProduct(product);
                        //get credential
                        listCredential.AddRange(product.Credentials);
                    }
                    // order item batch
                    if (orderItem.IsBatch)
                    {
                        // update inventory
                        var batch = await _batchRepository.GetBatchById(orderItem.BatchId);
                        batch.Inventory -= orderItem.Quantity;
                        if (batch.Inventory == 0)
                        {
                            batch.Status = ProductStatusEnum.SoldOut;
                        }
                        await _batchRepository.UpdateBatch(batch);
                    }
                }
                order.Status = OrderStatusEnum.Completed;
                if (result)
                {
                    await _orderRepository.UpdateOrder(order);
                    response.StatusCode = 201;
                    response.Message = "Payment created successfully";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = listCredential
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Payment creation failed";
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

        public async Task<ResponseDto> CreatePaymentByOrderIdCOD(Guid orderId)
        {
            var response = new ResponseDto();
            try
            {
                var order = await _orderRepository.GetOrderById(orderId);
                if (order.Status != OrderStatusEnum.Delivered)
                {
                    response.StatusCode = 400;
                    response.Message = "Order has not been delivered";
                    response.IsSuccess = false;
                    return response;
                }
                if (order.PaymentMethod != PaymentMethodEnum.COD)
                {
                    response.StatusCode = 400;
                    response.Message = "Payment method is not Cash On Delivery";
                    response.IsSuccess = false;
                    return response;
                }
                // create payment
                var payment = new PaymentOrder
                {
                    OrderId = orderId,
                    PaymentType = "Order",
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
                var listCredential = new List<Credential>();
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
                        // order item product
                        if (!orderItem.IsBatch)
                        {
                            //update inventory
                            var product = await _productRepository.GetProductById(orderItem.ProductId);
                            product.Inventory -= orderItem.Quantity;
                            if (product.Inventory == 0)
                            {
                                product.Status = ProductStatusEnum.SoldOut;
                            }
                            await _productRepository.UpdateProduct(product);
                            //get credential
                            listCredential.AddRange(product.Credentials);
                        }
                        // order item batch
                        if (orderItem.IsBatch)
                        {
                            // update inventory
                            var batch = await _batchRepository.GetBatchById(orderItem.BatchId);
                            batch.Inventory -= orderItem.Quantity;
                            if (batch.Inventory == 0)
                            {
                                batch.Status = ProductStatusEnum.SoldOut;
                            }
                            await _batchRepository.UpdateBatch(batch);
                        }
                    }
                    order.Status = OrderStatusEnum.Completed;
                    response.StatusCode = 201;
                    response.Message = "Payment created successfully";
                    response.Result = new ResultDto
                    {
                        Data = listCredential
                    };
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
                if (order.Shipment != null)
                {
                    order.Shipment.Status = ShipmentStatusEnum.Completed;
                }
                await _shipmentRepository.UpdateShipment(order.Shipment!);
                await _cartRepository.UpdateCart(cart);
                await _orderRepository.UpdateOrder(order);
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

        public async Task<ResponseDto> DeletePayment(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var payment = await _paymentRepository.GetPaymentById(id);
                if (payment == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Payment not found";
                    response.IsSuccess = false;
                    return response;
                }
                var result = await _paymentRepository.DeletePayment(payment);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Payment deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Payment deletion failed";
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

        public async Task<ResponseDto> GetOwnPayment()
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
                var payments = await _paymentRepository.GetPaymentsByUserId(userId);
                var mappedPayments = _mapper.Map<IEnumerable<PaymentDto>>(payments);
                if (payments != null && payments.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Payments found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedPayments
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Payments not found";
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

        public async Task<ResponseDto> GetPaymentById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var payment = await _paymentRepository.GetPaymentById(id);
                var mappedPayment = _mapper.Map<PaymentDto>(payment);
                if (payment != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Payment found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedPayment
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Payment not found";
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

        public async Task<ResponseDto> GetPaymentByUser(Guid userId)
        {
            var response = new ResponseDto();
            try
            {
                var payment = await _paymentRepository.GetPaymentsByUserId(userId);
                var mappedPayments = _mapper.Map<IEnumerable<PaymentDto>>(payment);
                if (payment != null && payment.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Payments found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedPayments
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Payments not found";
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

        public async Task<ResponseDto> GetPayments(PaymentQuery paymentQuery)
        {
            var response = new ResponseDto();
            try
            {
                var payments = await _paymentRepository.GetPayments(paymentQuery);
                var mappedPayments = _mapper.Map<IEnumerable<PaymentDto>>(payments.List);
                if (payments != null && payments.List.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Payments found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedPayments,
                        PaginationResp = new PaginationResp
                        {
                            Page = paymentQuery.Page,
                            PageSize = paymentQuery.PageSize,
                            Total = payments.Total
                        }
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Payments not found";
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