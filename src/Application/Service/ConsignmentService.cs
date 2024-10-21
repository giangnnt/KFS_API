using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Hangfire.Common;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.ConsignmentDtos;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.VNPay;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;

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
        public ConsignmentService(IOrderRepository orderRepository, IConsignmentRepository consignmentRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, IOrderItemRepository orderItemRepository, IMapper mapper, IBackgroundJobClient backgroundJobClient, IVNPayService vNPayService, IPaymentRepository paymentRepository)
        {
            _consignmentRepository = consignmentRepository;
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
            _vNPayService = vNPayService;
            _paymentRepository = paymentRepository;
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
                    Method = ConsignmentMethodEnum.Offline,
                    CommissionPercentage = req.CommissionPercentage,
                    DealingAmount = req.DealingAmount,
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
                        Species = req.Species,
                        Color = req.Color,
                        FeedingVolumn = req.FeedingVolumn,
                        FilterRate = req.FilterRate,
                        Inventory = req.Quantity,
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
                var product = orderItem.Product;
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
                // set to 0 if not for sell
                if (req.IsForSell != true)
                {
                    req.CommissionPercentage = 0;
                    req.DealingAmount = 0;
                }
                // check if order item is batch
                var consignment = new Consignment();
                if (orderItem.IsBatch)
                {
                    consignment.UserId = order.UserId;
                    consignment.Method = ConsignmentMethodEnum.Online;
                    consignment.CommissionPercentage = req.CommissionPercentage;
                    consignment.ConsignmentFee = req.ConsignmentFee;
                    consignment.ExpiryDate = req.ExpiryDate;
                    consignment.DealingAmount = req.DealingAmount;
                    consignment.Status = ConsignmentStatusEnum.Pending;
                    consignment.IsForSell = req.IsForSell;
                    consignment.User = order.User;
                    consignment.IsBatch = true;
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
                        Species = product.Species,
                        Color = product.Color,
                        FeedingVolumn = product.FeedingVolumn,
                        FilterRate = product.FilterRate,
                        Inventory = 0,
                        Status = ProductStatusEnum.Consignment,
                    };
                    foreach (var batch in product.Batches)
                    {
                        consignment.Product.Batches.Add(new Batch
                        {
                            Name = batch.Name,
                            Description = batch.Description,
                            Price = req.DealingAmount,
                            Quantity = batch.Quantity,
                            Inventory = batch.Inventory,
                            Status = ProductStatusEnum.Consignment,
                            IsForSell = false,
                        });
                    }
                }
                else
                {
                    consignment.UserId = order.UserId;
                    consignment.Method = ConsignmentMethodEnum.Online;
                    consignment.CommissionPercentage = req.CommissionPercentage;
                    consignment.ConsignmentFee = req.ConsignmentFee;
                    consignment.ExpiryDate = req.ExpiryDate;
                    consignment.DealingAmount = req.DealingAmount;
                    consignment.Status = ConsignmentStatusEnum.Pending;
                    consignment.IsForSell = req.IsForSell;
                    consignment.User = order.User;
                    consignment.IsBatch = false;
                    consignment.Product = new Product
                    {
                        IsForSell = false,
                        Name = product.Name,
                        Description = product.Description,
                        Price = req.DealingAmount,
                        Origin = product.Origin,
                        Category = product.Category,
                        CategoryId = product.CategoryId,
                        Age = product.Age,
                        Length = product.Length,
                        Species = product.Species,
                        Color = product.Color,
                        FeedingVolumn = product.FeedingVolumn,
                        FilterRate = product.FilterRate,
                        Status = ProductStatusEnum.Consignment,
                    };
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
                var consignment = await _consignmentRepository.GetConsignmentById(id);
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
                consignment.Status = isApproved ? ConsignmentStatusEnum.Approved : ConsignmentStatusEnum.Rejected;
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

        public async Task<ResponseDto> GetConsignments()
        {
            var response = new ResponseDto();
            try
            {
                var consignments = await _consignmentRepository.GetConsignments();
                var mappedConsignment = _mapper.Map<List<ConsignmentDto>>(consignments);
                if (consignments != null && consignments.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignments found";
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
                var consignment = await _consignmentRepository.GetConsignmentById(consignmentId);
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

        public Task<ResponseDto> UpdateConsignment(Guid id)
        {
            throw new NotImplementedException();
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
                    var payment = new PaymentConsignment
                    {
                        Id = Guid.NewGuid(),
                        PaymentType = "Consignment",
                        Amount = decimal.Parse(vnPayResponseModel.Amount) / 100,
                        CreatedAt = DateTime.Now,
                        Currency = "VND",
                        ConsignmentId = Guid.Parse(vnPayResponseModel.OrderId),
                        PaymentMethod = PaymentMethodEnum.VNPAY,
                        Status = PaymentStatusEnum.Failed,
                        TransactionId = vnPayResponseModel.PaymentId,
                        UserId = _consignmentRepository.GetConsignmentById(Guid.Parse(vnPayResponseModel.OrderId)).Result.UserId
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
                    _backgroundJobClient.Enqueue(() => JobExpireConsignment(Guid.Parse(vnPayResponseModel.OrderId)));
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
                    if (consignment.IsBatch)
                    {
                        foreach (var batch in product.Batches)
                        {
                            batch.IsForSell = true;
                        }
                    }
                }
                else
                {
                    product.IsForSell = false;
                    product.Status = ProductStatusEnum.Deactive;
                    if (consignment.IsBatch)
                    {
                        foreach (var batch in product.Batches)
                        {
                            batch.IsForSell = false;
                            batch.Status = ProductStatusEnum.Deactive;
                        }
                    }
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
        protected async Task JobExpireConsignment(Guid consignmentId)
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

        public async Task<ResponseDto> GetConsignmentByUserId(Guid userId)
        {
            var response = new ResponseDto();
            try
            {
                var consignments = await _consignmentRepository.GetConsignments();
                consignments = consignments.Where(x => x.UserId == userId).ToList();
                var mappedConsignment = _mapper.Map<List<ConsignmentDto>>(consignments);
                if (consignments != null && consignments.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignments found";
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

        public async Task<ResponseDto> GetOwnConsignment()
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
                var consignments = await _consignmentRepository.GetConsignmentsByUserId(payload.UserId);
                var mappedConsignment = _mapper.Map<List<ConsignmentDto>>(consignments);
                if (consignments != null && consignments.Count() > 0)
                {
                    response.StatusCode = 200;
                    response.Message = "Consignments found";
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