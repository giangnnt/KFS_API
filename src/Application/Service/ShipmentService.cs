using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.Pagination;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.ShipmentDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using KFS.src.Infrastucture.Context;
using static KFS.src.Application.Dto.Pagination.Pagination;

namespace KFS.src.Application.Service
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public ShipmentService(IShipmentRepository shipmentRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _shipmentRepository = shipmentRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto> ShipmentDelivered(Guid id, bool IsSuccess)
        {
            var response = new ResponseDto();
            try
            {
                var shipment = await _shipmentRepository.GetShipmentById(id);
                var order = await _orderRepository.GetOrderById(shipment.OrderId);
                if (shipment == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Shipment not found";
                    response.IsSuccess = false;
                    return response;
                }
                // Check shipment is success or not
                if (!IsSuccess)
                {
                    shipment.Status = ShipmentStatusEnum.Failed;
                    order.Status = OrderStatusEnum.Failed;
                    var result1 = await _shipmentRepository.UpdateShipment(shipment);
                    if (result1)
                    {
                        response.StatusCode = 400;
                        await _orderRepository.UpdateOrder(order);
                        response.Message = "Shipment failed";
                        response.IsSuccess = true;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Shipment failed";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                // Check order status
                if (order.Status != OrderStatusEnum.Delivering)
                {
                    response.StatusCode = 401;
                    response.Message = "Order is not Delivering";
                    response.IsSuccess = false;
                    return response;
                }
                shipment.Status = ShipmentStatusEnum.Delivered;
                order.Status = OrderStatusEnum.Delivered;
                var result = await _shipmentRepository.UpdateShipment(shipment);
                if (result)
                {
                    response.StatusCode = 200;
                    await _orderRepository.UpdateOrder(order);
                    response.Message = "Shipment Delivered successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Shipment Delivered failed";
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

        public async Task<ResponseDto> CreateShipment(Guid orderId)
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

                if (order.PaymentMethod != PaymentMethodEnum.COD && order.PaymentMethod != PaymentMethodEnum.VNPAY)
                {
                    response.StatusCode = 400;
                    response.Message = "Invalid payment method";
                    response.IsSuccess = false;
                    return response;
                }

                // Check order payment method
                if (order.Status != OrderStatusEnum.Accepted)
                {
                    response.StatusCode = 401;
                    response.Message = "Order is not Accepted";
                    response.IsSuccess = false;
                    return response;
                }

                order.Status = OrderStatusEnum.Delivering;
                await _orderRepository.UpdateOrder(order);

                var shipment = new Shipment
                {
                    OrderId = orderId,
                    Status = ShipmentStatusEnum.Delivering,
                };
                shipment.Order = order;
                var result = await _shipmentRepository.CreateShipment(shipment);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Shipment created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                response.StatusCode = 400;
                response.Message = "Shipment creation failed";
                response.IsSuccess = false;
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

        public async Task<ResponseDto> DeleteShipment(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var shipment = await _shipmentRepository.GetShipmentById(id);
                if (shipment == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Shipment not found";
                    response.IsSuccess = false;
                    return response;
                }
                var result = await _shipmentRepository.DeleteShipment(id);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Shipment deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                response.Message = "Shipment deletion failed";
                response.IsSuccess = false;
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

        public async Task<ResponseDto> GetShipmentById(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var shipment = await _shipmentRepository.GetShipmentById(id);
                var mappedShipment = _mapper.Map<ShipmentDto>(shipment);
                if (shipment != null)
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedShipment
                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "Shipment not found";
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

        public async Task<ResponseDto> GetShipments(ShipmentQuery shipmentQuery)
        {
            var response = new ResponseDto();
            try
            {
                var shipments = await _shipmentRepository.GetShipments(shipmentQuery);
                var mappedShipments = _mapper.Map<IEnumerable<ShipmentDto>>(shipments.List);

                if (shipments != null && shipments.List.Any())
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedShipments,
                        PaginationResp = new PaginationResp
                        {
                            Page = shipmentQuery.Page,
                            PageSize = shipmentQuery.PageSize,
                            Total = shipments.Total
                        }

                    };
                    return response;
                }
                else
                {
                    response.StatusCode = 404;
                    response.Message = "No shipments found";
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

        public async Task<ResponseDto> UpdateShipment(UpdateDto updateDto, Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var shipment = await _shipmentRepository.GetShipmentById(id);
                if (shipment == null)
                {
                    response.StatusCode = 404;
                    response.Message = "Shipment not found";
                    response.IsSuccess = false;
                    return response;
                }
                shipment.Status = updateDto.Status;

                var order = await _orderRepository.GetOrderById(shipment.OrderId);
                order.Status = (OrderStatusEnum)shipment.Status;
                await _orderRepository.UpdateOrder(order);

                var result = await _shipmentRepository.UpdateShipment(shipment);
                if (result)
                {
                    response.StatusCode = 200;
                    response.Message = "Shipment updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                response.Message = "Shipment update failed";
                response.IsSuccess = false;
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

        public async Task<ResponseDto> ShipmentCompleted(Guid id, bool IsSuccess)
        {
            var response = new ResponseDto();
            try
            {
                var shipment = await _shipmentRepository.GetShipmentById(id);
                var order = await _orderRepository.GetOrderById(shipment.OrderId);
                // check is success
                if (!IsSuccess)
                {
                    shipment.Status = ShipmentStatusEnum.Failed;
                    order.Status = OrderStatusEnum.Failed;
                    var result1 = await _shipmentRepository.UpdateShipment(shipment);
                    if (result1)
                    {
                        response.StatusCode = 400;
                        await _orderRepository.UpdateOrder(order);
                        response.Message = "Shipment failed";
                        response.IsSuccess = true;
                        return response;
                    }
                    else
                    {
                        response.StatusCode = 400;
                        response.Message = "Shipment failed";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                // complete for vn pay
                if (order.PaymentMethod != PaymentMethodEnum.VNPAY || shipment.Status != ShipmentStatusEnum.Delivered)
                {
                    response.Message = "Cannot complete shipment";
                    response.IsSuccess = false;
                    return response;
                }
                shipment.Status = ShipmentStatusEnum.Completed;
                order.Status = OrderStatusEnum.Completed;
                var result = await _shipmentRepository.UpdateShipment(shipment);
                if (result)
                {
                    response.StatusCode = 200;
                    await _orderRepository.UpdateOrder(order);
                    response.Message = "Shipment completed successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.StatusCode = 400;
                    response.Message = "Shipment completion failed";
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