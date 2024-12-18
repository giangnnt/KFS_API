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

                // validate cart item before checkout
                foreach (var cartItem in cart.CartItems)
                {
                    if (cartItem is CartItemProduct productItem)
                    {
                        var product = await _productRepository.GetProductById(productItem.ProductId);
                        if (product.Status != ProductStatusEnum.Active && product.Status != ProductStatusEnum.Consignment)
                        {
                            response.StatusCode = 400;
                            response.Message = $"Product: {product.Name} is not available";
                            response.IsSuccess = false;
                            return response;
                        }
                    }
                    if (cartItem is CartItemBatch batchItem)
                    {
                        var batch = await _batchRepository.GetBatchById(batchItem.BatchId);
                        if (batch.Status != ProductStatusEnum.Active)
                        {
                            response.StatusCode = 400;
                            response.Message = $"Batch: {batch.Name} is not available";
                            response.IsSuccess = false;
                            return response;
                        }
                    }
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
                    response.Result = new ResultDto
                    {
                        Data = _configuration["VNPay:FailUrl"]
                    };
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

                        //update status
                        foreach (var orderItem in order.OrderItems)
                        {
                            if (orderItem is OrderItemProduct productItem)
                            {
                                var product = await _productRepository.GetProductById(productItem.ProductId);
                                product.Status = ProductStatusEnum.SoldOut;
                                await _productRepository.UpdateProduct(product);
                            }
                            if (orderItem is OrderItemBatch batchItem)
                            {
                                var batch = await _batchRepository.GetBatchById(batchItem.BatchId);
                                batch.Status = ProductStatusEnum.SoldOut;
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
                if (mappedOrders != null && orders.Count() > 0)
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

        public async Task<ResponseDto> GetOrders(OrderQuery req)
        {
            var response = new ResponseDto();
            try
            {
                var orders = await _orderRepository.GetOrders(req);
                var mappedOrders = _mapper.Map<List<OrderDto>>(orders.List);
                if (mappedOrders != null && mappedOrders.Count() > 0)
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
                    response.StatusCode = 402;
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

                // check if product is available
                if (req.ProductIds != null && req.ProductIds.Count() > 0)
                {
                    foreach (var productId in req.ProductIds)
                    {
                        var product = await _productRepository.GetProductById(productId);
                        if (product == null)
                        {
                            response.StatusCode = 404;
                            response.Message = "Product not found";
                            response.IsSuccess = false;
                            return response;
                        }
                        if (product.Status != ProductStatusEnum.Active && product.Status != ProductStatusEnum.Consignment)
                        {
                            response.StatusCode = 400;
                            response.Message = "Product is not available";
                            response.IsSuccess = false;
                            return response;
                        }
                        if (product.IsForSell == false)
                        {
                            response.StatusCode = 400;
                            response.Message = "Product is not available";
                            response.IsSuccess = false;
                            return response;
                        }
                        // add order item
                        var orderItem = new OrderItemProduct
                        {
                            Id = Guid.NewGuid(),
                            ProductId = product.Id,
                            Price = product.Price,
                        };
                        orderItems.Add(orderItem);
                    }
                }
                // check if batch is available
                if (req.BatchIds != null && req.BatchIds.Count() > 0)
                {
                    foreach (var batchId in req.BatchIds)
                    {
                        var batch = await _batchRepository.GetBatchById(batchId);
                        if (batch == null)
                        {
                            response.StatusCode = 404;
                            response.Message = "Batch not found";
                            response.IsSuccess = false;
                            return response;
                        }
                        if (batch.Status != ProductStatusEnum.Active)
                        {
                            response.StatusCode = 400;
                            response.Message = "Batch is not available";
                            response.IsSuccess = false;
                            return response;
                        }
                        if (batch.IsForSell == false)
                        {
                            response.StatusCode = 400;
                            response.Message = "Batch is not available";
                            response.IsSuccess = false;
                            return response;
                        }
                        // add order item
                        var orderItem = new OrderItemBatch
                        {
                            Id = Guid.NewGuid(),
                            BatchId = batch.Id,
                            Price = batch.Price,
                        };
                        orderItems.Add(orderItem);
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
                    TotalPrice = orderItems.Sum(x => x.Price),
                    TotalItem = orderItems.Count(),
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

                if (mappedOrders != null && mappedOrders.Count() > 0)
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