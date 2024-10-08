using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Core.Jwt;
using KFS.src.Application.Dto.ConsignmentDtos;
using KFS.src.Application.Dto.ResponseDtos;
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
        public ConsignmentService(IOrderRepository orderRepository, IConsignmentRepository consignmentRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _consignmentRepository = consignmentRepository;
            _orderRepository = orderRepository;
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
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
            catch
            {
                throw;
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
                // create consignment
                var consignment = new Consignment
                {
                    UserId = order.UserId,
                    Method = ConsignmentMethodEnum.Online,
                    CommissionPercentage = req.CommissionPercentage,
                    DealingAmount = req.DealingAmount,
                    Status = ConsignmentStatusEnum.Pending,
                    IsForSell = req.IsForSell,
                    Product = new Product
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
                        Gender = product.Gender,
                        Inventory = orderItem.Quantity,
                        CreatedAt = DateTime.Now,
                    }
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
                // update product status
                if (consignment.IsForSell)
                {
                    consignment.Product.IsForSell = isApproved;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
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
            catch
            {
                throw;
            }
        }

        public Task<ResponseDto> UpdateConsignment(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}