using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.GHN;
using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.VNPay;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using static KFS.src.Application.Dto.Pagination.Pagination;

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
        private readonly HttpContext _httpContext;
        private readonly IOwnerService _ownerService;
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IGHNService _ghnService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, IUserRepository userRepository, ICartRepository cartRepository, IVNPayService vNPayService, IHttpContextAccessor httpContextAccessor, IProductRepository productRepository, IPaymentRepository paymentRepository, IWalletRepository walletRepository, IPromotionRepository promotionRepository, IBatchRepository batchRepository, IOwnerService ownerService, IShipmentRepository shipmentRepository, IAddressRepository addressRepository, IGHNService ghnService, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _vNPayService = vNPayService;
            _paymentRepository = paymentRepository;
            _productRepository = productRepository;
            _walletRepository = walletRepository;
            _promotionRepository = promotionRepository;
            _batchRepository = batchRepository;
            _ownerService = ownerService;
            _shipmentRepository = shipmentRepository;
            _addressRepository = addressRepository;
            _ghnService = ghnService;
            _configuration = configuration;

            // http context
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("Http context is required.");
        }

        public async Task<ResponseDto> CreateOrderFromCart(Guid id, OrderCreateFromCart req)
        {
            var response = new ResponseDto();
            try
            {
                //map order create to order
                var mappedOrder = _mapper.Map<Order>(req);

                //check if cart exist
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
                //check if cart is active
                if (cart.Status != CartStatusEnum.Active && cart.Status != CartStatusEnum.Completed)
                {
                    response.StatusCode = 400;
                    response.Message = "Cart is not active or completed";
                    response.IsSuccess = false;
                    return response;
                }
                if (cart.Status == CartStatusEnum.Completed)
                {
                    mappedOrder.IsReBuy = true;
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

                // check shipping address
                var address = await _addressRepository.GetAddressById(mappedOrder.AddressId);
                if (address == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Address not found";
                    response.IsSuccess = false;
                    return response;
                }
                mappedOrder.Address = address;

                // check contact number
                if (mappedOrder.ContactNumber == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Contact number is required";
                    response.IsSuccess = false;
                    return response;
                }

                // check contact name
                if (mappedOrder.ContactName == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Contact name is required";
                    response.IsSuccess = false;
                    return response;
                }

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
                var GHNResponse = await _ghnService.CalculateShippingFee(new GHNRequest
                {
                    ToWardCode = address.WardCode,
                    ToDistrictId = address.DistrictId,
                    Weight = mappedOrder.TotalWeight,
                    ServiceId = mappedOrder.ServiceId,
                    ServiceTypeId = mappedOrder.ServiceTypeId
                });
                if (GHNResponse != null)
                {
                    mappedOrder.ShippingFee = GHNResponse.Data.Total;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Can not calculate shipping fee";
                    response.IsSuccess = false;
                    return response;
                }

                // Use point from wallet
                if (req.UsePoint == true)
                {
                    mappedOrder.IsUsePoint = true;
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
                        Data = new
                        {
                            mappedOrder.ShippingFee,
                            paymentUrl = data
                        }
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
                    FullName = order.ContactName!,
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

                    response.StatusCode = 400;
                    response.Message = "Payment creation failed" + vnPayResponseModel.VnPayResponseCode;
                    response.IsSuccess = false;
                    return response;
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
                    //create payment
                    var result = await _paymentRepository.CreatePayment(payment);
                    var listCredential = new List<Credential>();
                    if (result)
                    {
                        if (order.IsUsePoint == true)
                        {
                            var wallet = await _walletRepository.GetWalletByUserId(payment.UserId);
                            await _walletRepository.AddPoint(wallet.Id, (int)payment.Amount * 5 / 100);
                        }
                        
                        // check if order is !re-buy
                        if (!order.IsReBuy)
                        {
                            // deactive cart
                            foreach (var c in carts)
                            {
                                if (c.Status == CartStatusEnum.Active)
                                {
                                    c.Status = CartStatusEnum.Completed;
                                    cart = c;
                                }
                            }
                            // create new active cart
                            var newCart = new Cart
                            {
                                UserId = order.UserId,
                                Status = CartStatusEnum.Active,
                            };
                            await _cartRepository.CreateCart(newCart);
                        }

                        //update inventory
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
                        order.Status = OrderStatusEnum.Paid;
                        response.StatusCode = 200;
                        response.Message = "Payment created successfully";
                        response.Result = new ResultDto
                        {
                            Data = _configuration["VNPay:RedirectUrl"]
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
                //check if order is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, order.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
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
                    orderItem.Product = _mapper.Map<ProductDtoNoBatch>(product);
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
                        orderItem.Product = _mapper.Map<ProductDtoNoBatch>(product);
                    }
                }
            }
        }

        public async Task<ResponseDto> GetOrders(OrderQuery req)
        {
            var response = new ResponseDto();
            try
            {
                var orders = await _orderRepository.GetOrders(req);
                var mappedOrders = _mapper.Map<List<OrderDto>>(orders.List);
                //map by format
                await GetOrdersFormat(mappedOrders);

                if (orders != null && orders.List.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Orders found";
                    response.Result = new ResultDto
                    {
                        Data = mappedOrders,
                        PaginationResp = new PaginationResp
                        {
                            Page = req.Page,
                            PageSize = req.PageSize,
                            Total = orders.Total
                        }
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
                //check if order is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, order.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
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

        public async Task<ResponseDto> AcceptOrder(Guid id, bool isAccept)
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
                if (order.PaymentMethod == PaymentMethodEnum.VNPAY && order.Status != OrderStatusEnum.Paid)
                {
                    response.StatusCode = 404;
                    response.Message = "Order status is not Paid";
                    response.IsSuccess = false;
                    return response;
                }
                if (order.PaymentMethod == PaymentMethodEnum.COD && order.Status != OrderStatusEnum.Processing)
                {
                    response.StatusCode = 404;
                    response.Message = "Order status is not Processing";
                    response.IsSuccess = false;
                    return response;
                }
                if (order.Status == OrderStatusEnum.Delivering || order.Status == OrderStatusEnum.Delivered || order.Status == OrderStatusEnum.Completed || order.Status == OrderStatusEnum.Canceled || order.Status == OrderStatusEnum.Failed)
                {
                    response.StatusCode = 400;
                    response.Message = "Can not update order";
                    response.IsSuccess = false;
                    return response;
                }
                //set order status
                order.Status = isAccept ? OrderStatusEnum.Accepted : OrderStatusEnum.Canceled;

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

        public async Task<ResponseDto> CreateOrderOffline(OrderCreateOffline req)
        {
            var response = new ResponseDto();
            try
            {
                var orderItems = new List<OrderItem>();
                var products = new List<Product>();
                var batches = new List<Batch>();
                // get http context
                var HttpContext = _httpContextAccessor.HttpContext;
                if (HttpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "HttpContext is null";
                    response.IsSuccess = false;
                    return response;
                }
                // get payload
                var payload = HttpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Payload is null";
                    response.IsSuccess = false;
                    return response;
                }

                // check product and batch empty
                if ((req.ProductsReq == null || req.ProductsReq.Count() == 0) && (req.BatchesReq == null || req.BatchesReq.Count() == 0))
                {
                    response.StatusCode = 400;
                    response.Message = "Product or Batch is required";
                    response.IsSuccess = false;
                    return response;
                }

                // create order items product
                if (req.ProductsReq != null && req.ProductsReq.Count() > 0)
                {
                    foreach (var productReq in req.ProductsReq)
                    {
                        if (productReq.Quantity == 0)
                        {
                            response.StatusCode = 400;
                            response.Message = "Quantity must be greater than 0";
                            response.IsSuccess = false;
                            return response;
                        }
                        var product = await _productRepository.GetProductById(productReq.ProductId);
                        var orderItem = new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            ProductId = product.Id,
                            Quantity = productReq.Quantity,
                            Price = product.Price,
                            IsBatch = false,
                            IsConsignment = false
                        };
                        products.Add(product);
                        orderItems.Add(orderItem);
                    }
                }

                // create order items batch
                if (req.BatchesReq != null && req.BatchesReq.Count() > 0)
                {
                    foreach (var batchReq in req.BatchesReq)
                    {
                        if (batchReq.Quantity == 0)
                        {
                            response.StatusCode = 400;
                            response.Message = "Quantity must be greater than 0";
                            response.IsSuccess = false;
                            return response;
                        }
                        var batch = await _batchRepository.GetBatchById(batchReq.BatchId);
                        var orderItem = new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            BatchId = batch.Id,
                            ProductId = batch.ProductId,
                            Quantity = batchReq.Quantity,
                            Price = batch.Price,
                            IsBatch = true,
                            IsConsignment = false
                        };
                        batches.Add(batch);
                        orderItems.Add(orderItem);
                    }
                }
                // check product and batch duplicate
                if (req.ProductsReq != null && req.BatchesReq != null)
                {
                    foreach (var batch in batches)
                    {
                        if (products.Any(x => x.Id == batch.ProductId))
                        {
                            response.StatusCode = 400;
                            response.Message = "You can only choose one out of two: product or cart";
                            response.IsSuccess = false;
                            return response;
                        }
                    }
                }

                // check shipping address
                var address = await _addressRepository.GetAddressById(Guid.Parse("00000000-0000-0000-0000-000000000001"));

                // create order
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    AddressId = address.Id,
                    UserId = payload.UserId,
                    TotalPrice = orderItems.Sum(x => x.Price * x.Quantity),
                    TotalItem = orderItems.Sum(x => x.Quantity),
                    TotalWeight = 0,
                    OrderItems = orderItems,
                    Status = OrderStatusEnum.Processing,
                    Currency = req.Currency,
                    ShippingFee = 0,
                    PaymentMethod = PaymentMethodEnum.OFFLINE,
                };
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
                        order.Discount = targetPromotion.DiscountPercentage;
                    }
                }
                var result = await _orderRepository.CreateOrder(order);
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
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
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

        public Task<ResponseDto> CancelOrder(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                //get order
                var order = _orderRepository.GetOrderById(id).Result;
                if (order == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                    return Task.FromResult(response);
                }
                if (order.Status == OrderStatusEnum.Accepted || order.Status == OrderStatusEnum.Delivering || order.Status == OrderStatusEnum.Delivered || order.Status == OrderStatusEnum.Completed || order.Status == OrderStatusEnum.Canceled || order.Status == OrderStatusEnum.Failed)
                {
                    response.StatusCode = 400;
                    response.Message = "Can not cancel order";
                    response.IsSuccess = false;
                    return Task.FromResult(response);
                }
                //set order status
                order.Status = OrderStatusEnum.Canceled;

                //update order
                var result = _orderRepository.UpdateOrder(order).Result;

                //check result
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Order status updated successfully";
                    response.IsSuccess = true;
                    return Task.FromResult(response);
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Order status update failed";
                    response.IsSuccess = false;
                    return Task.FromResult(response);
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return Task.FromResult(response);
            }
        }

        public async Task<ResponseDto> OrderReturn(Guid orderId)
        {
            var response = new ResponseDto();
            try
            {
                //get order
                var order = await _orderRepository.GetOrderById(orderId);
                if (order == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                    return response;
                }
                if (order.Shipment == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Shipment not found";
                    response.IsSuccess = false;
                    return response;
                }

                if (order.Status != OrderStatusEnum.Delivered || order.Shipment.Status != ShipmentStatusEnum.Delivered)
                {
                    response.StatusCode = 400;
                    response.Message = "Can not return order";
                    response.IsSuccess = false;
                    return response;
                }

                //set order status
                order.Status = OrderStatusEnum.Canceled;
                //set shipment status
                order.Shipment.Status = ShipmentStatusEnum.Cancelled;

                //update order
                var result = _orderRepository.UpdateOrder(order).Result;

                //check result
                if (result)
                {
                    await _shipmentRepository.UpdateShipment(order.Shipment);
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
    }
}