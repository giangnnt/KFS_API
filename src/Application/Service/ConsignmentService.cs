using AutoMapper;
using Hangfire;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.ConsignmentDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.VNPay;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Service
{
    public class ConsignmentService : IConsignmentService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IConsignmentRepository _consignmentRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IVNPayService _vNPayService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IOwnerService _ownerService;
        public ConsignmentService(IOrderRepository orderRepository, IConsignmentRepository consignmentRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, IOrderItemRepository orderItemRepository, IMapper mapper, IBackgroundJobClient backgroundJobClient, IVNPayService vNPayService, IPaymentRepository paymentRepository, IBatchRepository batchRepository, IOwnerService ownerService)
        {
            _consignmentRepository = consignmentRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
            _vNPayService = vNPayService;
            _paymentRepository = paymentRepository;
            _batchRepository = batchRepository;
            _ownerService = ownerService;

            // http context
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto> CreateConsignment(ConsignmentCreate req)
        {
            var response = new ResponseDto();
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                response.StatusCode = 400;
                response.Message = "Unauthorized";
                response.IsSuccess = false;
                return response;
            }
            var payload = httpContext.Items["payload"] as Payload;
            if (payload == null)
            {
                response.StatusCode = 400;
                response.Message = "Unauthorized";
                response.IsSuccess = false;
                return response;
            }
            try
            {
                // set to 0 if not for sell
                if (req.IsForSell != true)
                {
                    req.CommissionPercentage = 0;
                    req.DealingAmount = 0;
                }
                var consignment = new Consignment
                {
                    UserId = payload.UserId,
                    Method = req.Method,
                    CommissionPercentage = req.CommissionPercentage,
                    DealingAmount = req.DealingAmount,
                    ConsignmentFee = req.ConsignmentFee,
                    Status = ConsignmentStatusEnum.Pending,
                    IsForSell = req.IsForSell,
                    Product = new Product
                    {
                        IsForSell = false,
                        Name = req.Name,
                        Description = req.Description,
                        Price = req.DealingAmount,
                        Origin = req.Origin,
                        CategoryId = req.CategoryId,
                        Age = req.Age,
                        Length = req.Length,
                        Color = req.Color,
                        FeedingVolumn = req.FeedingVolumn,
                        FilterRate = req.FilterRate,
                        CreatedAt = DateTime.Now,
                    }
                };
                var result = await _consignmentRepository.CreateConsignment(consignment);
                if (result)
                {
                    response.StatusCode = 201;
                    response.Message = "Consignment created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Failed to create consignment";
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

        public async Task<ResponseDto> CreateConsignmentOnline(ConsignmentCreateByOrderItem req)
        {
            var response = new ResponseDto();
            try
            {
                var orderItem = await _orderItemRepository.GetOrderItemById(req.OrderItemId);
                var order = await _orderRepository.GetOrderById(req.OrderId);
                if (orderItem is not OrderItemProduct productItem)
                {
                    response.StatusCode = 400;
                    response.Message = "Order item is not product";
                    response.IsSuccess = false;
                    return response;
                }
                if (orderItem.IsConsignment)
                {
                    response.StatusCode = 400;
                    response.Message = "Order item is already consignment";
                    response.IsSuccess = false;
                    return response;
                }
                // get product
                var product = await _productRepository.GetProductById(productItem.ProductId);
                if (order == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                    return response;
                }
                // Check order payment method
                if (order.PaymentMethod != PaymentMethodEnum.VNPAY)
                {
                    response.StatusCode = 400;
                    response.Message = "Consignment can only be created for online payment order";
                    response.IsSuccess = false;
                    return response;
                }
                // Check if order is Paid
                if (order.Status != OrderStatusEnum.Paid)
                {
                    response.StatusCode = 400;
                    response.Message = "Consignment can only be created for paid order";
                    response.IsSuccess = false;
                    return response;
                }
                // Check if order item is in order
                if (order.OrderItems.Any(x => x.Id == req.OrderItemId) == false)
                {
                    response.StatusCode = 400;
                    response.Message = "Order item not found in order";
                    response.IsSuccess = false;
                    return response;
                }
                var consignment = new Consignment
                {
                    UserId = order.UserId,
                    OrderItemId = req.OrderItemId,
                    Method = ConsignmentMethodEnum.Caring,
                    CommissionPercentage = 0,
                    DealingAmount = 0,
                    ConsignmentFee = req.ConsignmentFee,
                    Status = ConsignmentStatusEnum.Pending,
                    IsForSell = false,
                    ExpiryDate = req.ExpiryDate,
                    ImageUrl = product.ImageUrl,
                };
                consignment.Product = new Product
                {
                    IsForSell = false,
                    Name = product.Name,
                    Description = product.Description,
                    Price = 0,
                    Origin = product.Origin,
                    Category = product.Category,
                    CategoryId = product.CategoryId,
                    Age = product.Age,
                    Length = product.Length,
                    Color = product.Color,
                    FeedingVolumn = product.FeedingVolumn,
                    FilterRate = product.FilterRate,
                    Gender = product.Gender,
                    Status = ProductStatusEnum.Consignment,
                };
                var result = await _consignmentRepository.CreateConsignment(consignment);
                if (result == false)
                {
                    response.StatusCode = 400;
                    response.Message = "Failed to create consignment";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    orderItem.IsConsignment = true;
                    await _orderItemRepository.UpdateOrderItem(orderItem);
                    response.StatusCode = 201;
                    response.Message = "Consignment created successfully";
                    response.IsSuccess = true;
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

        public async Task<ResponseDto> DeleteConsignment(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var _httpContext = _httpContextAccessor.HttpContext;
                if (_httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var consignment = await _consignmentRepository.GetConsignmentById(id);
                //check if consignment is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, consignment.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                if (consignment == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Consignment not found";
                    response.IsSuccess = false;
                    return response;
                }
                var result = await _consignmentRepository.DeleteConsignment(consignment);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignment deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Failed to delete consignment";
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

        public async Task<ResponseDto> EvaluateConsignment(bool isApproved, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var consignment = await _consignmentRepository.GetConsignmentById(id);
                if (consignment == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Consignment not found";
                    response.IsSuccess = false;
                    return response;
                }
                if (consignment.Status != ConsignmentStatusEnum.Pending)
                {
                    response.StatusCode = 400;
                    response.Message = "Consignment is not pending";
                    response.IsSuccess = false;
                    return response;
                }
                // update consignment status
                if (!isApproved)
                {
                    consignment.Status = ConsignmentStatusEnum.Rejected;
                    // update product
                    var product = consignment.Product;
                    product.Status = ProductStatusEnum.Deactive;
                    await _productRepository.UpdateProduct(product);
                    // update order item
                    if (consignment.OrderItemId != null)
                    {
                        var orderItem = await _orderItemRepository.GetOrderItemById(consignment.OrderItemId.Value);
                        orderItem.IsConsignment = false;
                        await _orderItemRepository.UpdateOrderItem(orderItem);
                    }
                }
                else
                {
                    consignment.Status = ConsignmentStatusEnum.Approved;
                }
                var result = await _consignmentRepository.UpdateConsignment(consignment);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignment evaluated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Failed to evaluate consignment";
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

        public async Task<ResponseDto> GetConsignmentById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var consignment = await _consignmentRepository.GetConsignmentById(id);
                var mappedConsignment = _mapper.Map<ConsignmentDto>(consignment);
                if (mappedConsignment != null)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignment found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedConsignment
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Consignment not found";
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

        public async Task<ResponseDto> GetConsignmentsAdmin(ConsignmentQuery consignmentQuery)
        {
            var response = new ResponseDto();
            try
            {
                var consignments = await _consignmentRepository.GetConsignmentsAdmin(consignmentQuery);
                var mappedConsignment = _mapper.Map<IEnumerable<ConsignmentDto>>(consignments.List);
                if (consignments != null && consignments.List.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignments found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedConsignment,
                        PaginationResp = new PaginationResp
                        {
                            Page = consignmentQuery.Page,
                            PageSize = consignmentQuery.PageSize,
                            Total = consignments.Total
                        }
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No consignment found";
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

        public async Task<ResponseDto> PayForConsignment(Guid consignmentId)
        {
            var response = new ResponseDto();
            try
            {
                var _httpContext = _httpContextAccessor.HttpContext;
                if (_httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var consignment = await _consignmentRepository.GetConsignmentById(consignmentId);
                //check if consignment is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, consignment.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                if (consignment == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Consignment not found";
                    response.IsSuccess = false;
                    return response;
                }
                if (consignment.Status != ConsignmentStatusEnum.Approved)
                {
                    response.StatusCode = 400;
                    response.Message = "Consignment is not approved";
                    response.IsSuccess = false;
                    return response;
                }
                var paymentUrl = GeneratePaymentUrl(consignment);
                response.StatusCode = 200;
                response.Message = "Payment url generated";
                response.IsSuccess = true;
                response.Result = new ResultDto
                {
                    Data = paymentUrl
                };
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

        public async Task<ResponseDto> UpdateConsignment(Guid id, ConsignmentUpdate req)
        {
            var response = new ResponseDto();
            try
            {
                var consignment = await _consignmentRepository.GetConsignmentById(id);
                if (consignment == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Consignment not found";
                    response.IsSuccess = false;
                    return response;
                }
                var _httpContext = _httpContextAccessor.HttpContext;
                if (_httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                //check if consignment is authorized
                var isOwner = _ownerService.CheckEntityOwner(_httpContext, consignment.UserId);
                if (!isOwner)
                {
                    response.StatusCode = 401;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                // check consignment status
                if (consignment.Status != ConsignmentStatusEnum.Pending)
                {
                    response.StatusCode = 400;
                    response.Message = "Consignment is not pending";
                    response.IsSuccess = false;
                    return response;
                }
                // update consignment
                var mappedConsignment = _mapper.Map(req, consignment);
                var result = await _consignmentRepository.UpdateConsignment(mappedConsignment);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignment updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Failed to update consignment";
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
        public string GeneratePaymentUrl(Consignment consignment)
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
                    Amount = consignment.ConsignmentFee,
                    CreateDate = DateTime.Now,
                    Description = "Payment for consignment " + consignment.Id.ToString(),
                    FullName = consignment.User.FullName,
                    OrderId = consignment.Id
                }, "consignment");
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
                    // get order
                    var order = await _orderRepository.GetOrderById(Guid.Parse(vnPayResponseModel.OrderId));
                    //update consignment order item status
                    foreach (var item in order.OrderItems)
                    {
                        if (item.IsConsignment)
                        {
                            item.IsConsignment = false;
                            await _orderItemRepository.UpdateOrderItem(item);
                        }
                    }
                    response.StatusCode = 400;
                    response.Message = "Payment creation failed" + vnPayResponseModel.VnPayResponseCode;
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    //create payment
                    var payment = new PaymentConsignment
                    {
                        Id = Guid.NewGuid(),
                        PaymentType = "Consignment",
                        Amount = decimal.Parse(vnPayResponseModel.Amount) / 100,
                        CreatedAt = DateTime.Now,
                        Currency = "VND",
                        ConsignmentId = Guid.Parse(vnPayResponseModel.OrderId),
                        PaymentMethod = PaymentMethodEnum.VNPAY,
                        Status = PaymentStatusEnum.Completed,
                        TransactionId = vnPayResponseModel.PaymentId,
                        UserId = _consignmentRepository.GetConsignmentById(Guid.Parse(vnPayResponseModel.OrderId)).Result.UserId
                    };
                    var consignment = await _consignmentRepository.GetConsignmentById(Guid.Parse(vnPayResponseModel.OrderId));
                    //update consignment status active
                    await SetStatusConsignment(ConsignmentStatusEnum.Active, Guid.Parse(vnPayResponseModel.OrderId));
                    //job for expire consignment
                    _backgroundJobClient.Schedule(() => JobExpireConsignment(Guid.Parse(vnPayResponseModel.OrderId)), consignment.ExpiryDate);
                    var result = await _paymentRepository.CreatePayment(payment);
                    if (result)
                    {
                        response.StatusCode = 200;
                        response.Message = "Payment created successfully";
                        response.IsSuccess = true;
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
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        protected async Task SetStatusConsignment(ConsignmentStatusEnum status, Guid id)
        {
            try
            {
                //update consignment status
                var consignment = await _consignmentRepository.GetConsignmentById(id);
                consignment.Status = status;
                //update product status
                var product = consignment.Product;
                if (status == ConsignmentStatusEnum.Active)
                {
                    product.IsForSell = true;
                    product.Status = ProductStatusEnum.Consignment;
                }
                else
                {
                    product.IsForSell = false;
                    product.Status = ProductStatusEnum.Deactive;
                    product.Status = ProductStatusEnum.Deactive;
                }
                var result = await _consignmentRepository.UpdateConsignment(consignment);
                if (result)
                {
                    Console.WriteLine("Consignment status updated");
                    return;
                }
                else
                {
                    Console.WriteLine("Failed to update consignment status");
                    return;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }
        public async Task JobExpireConsignment(Guid consignmentId)
        {
            try
            {
                await SetStatusConsignment(ConsignmentStatusEnum.Expired, consignmentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        public async Task<ResponseDto> GetConsignmentByUserId(ConsignmentQuery consignmentQuery, Guid userId)
        {
            var response = new ResponseDto();
            try
            {
                var consignments = await _consignmentRepository.GetConsignmentsByUserId(consignmentQuery, userId);
                var mappedConsignment = _mapper.Map<IEnumerable<ConsignmentDto>>(consignments.List);
                if (consignments != null && consignments.List.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignments found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedConsignment,
                        PaginationResp = new PaginationResp
                        {
                            Page = consignmentQuery.Page,
                            PageSize = consignmentQuery.PageSize,
                            Total = consignments.Total
                        }
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No consignment found";
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

        public async Task<ResponseDto> GetOwnConsignment(ConsignmentQuery consignmentQuery)
        {
            var response = new ResponseDto();
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var payload = httpContext.Items["payload"] as Payload;
                if (payload == null)
                {
                    response.StatusCode = 400;
                    response.Message = "Unauthorized";
                    response.IsSuccess = false;
                    return response;
                }
                var consignments = await _consignmentRepository.GetConsignmentsByUserId(consignmentQuery, payload.UserId);
                var mappedConsignment = _mapper.Map<IEnumerable<ConsignmentDto>>(consignments.List);
                if (consignments != null && consignments.List.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignments found";
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedConsignment,
                        PaginationResp = new PaginationResp
                        {
                            Page = consignmentQuery.Page,
                            PageSize = consignmentQuery.PageSize,
                            Total = consignments.Total
                        }
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No consignment found";
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