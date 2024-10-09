using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.ResponseDtos;
using KFS.src.Application.Dto.ShipmentDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using KFS.src.Domain.IRepository;
using KFS.src.Domain.IService;
using KFS.src.Infrastucture.Context;

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

        public async Task<ResponseDto> ShipmentDelivered(Guid id)
        {
            var response = new ResponseDto();
            try
            {
                var shipment = await _shipmentRepository.GetShipmentById(id);
                var order = await _orderRepository.GetOrderById(shipment.OrderId);
                if (shipment == null)
                {
                    response.Message = "Shipment not found";
                    response.IsSuccess = false;
                    return response;
                }
                // Check order payment method
                if (order.PaymentMethod == PaymentMethodEnum.VNPAY)
                {
                    order.Status = OrderStatusEnum.Completed;
                    shipment.Status = ShipmentStatusEnum.Completed;
                }
                else
                {
                    order.Status = OrderStatusEnum.Delivered;
                    shipment.Status = ShipmentStatusEnum.Delivered;
                }
                var result = await _shipmentRepository.UpdateShipment(shipment);
                if (result)
                {
                    await _orderRepository.UpdateOrder(order);
                    response.Message = "Shipment Delivered successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.Message = "Shipment Delivered failed";
                    response.IsSuccess = false;
                    return response;
                }
        }
            catch
            {
                throw;
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
                    response.Message = "Order not found";
                    response.IsSuccess = false;
                    return response;
                }
                // Check order payment method
                if (order.PaymentMethod == PaymentMethodEnum.VNPAY)
                {
                    if (order.Status != OrderStatusEnum.Paid)
                    {
                        response.Message = "Order is not paid";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                if (order.PaymentMethod == PaymentMethodEnum.COD)
                {
                    if (order.Status != OrderStatusEnum.Processing)
                    {
                        response.Message = "Order is not processing";
                        response.IsSuccess = false;
                        return response;
                    }
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
                    response.Message = "Shipment created successfully";
                    response.IsSuccess = true;
                    return response;
                }
                response.Message = "Shipment creation failed";
                response.IsSuccess = false;
                return response;

            }
            catch
            {
                throw;
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
                    response.Message = "Shipment not found";
                    response.IsSuccess = false;
                    return response;
                }
                var result = await _shipmentRepository.DeleteShipment(id);
                if (result)
                {
                    response.Message = "Shipment deleted successfully";
                    response.IsSuccess = true;
                    return response;
                }
                response.Message = "Shipment deletion failed";
                response.IsSuccess = false;
                return response;
            }
            catch
            {
                throw;
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
                    response.Message = "Shipment not found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<ResponseDto> GetShipments()
        {
            var response = new ResponseDto();
            try
            {
                var shipments = await _shipmentRepository.GetShipments();
                var mappedShipments = _mapper.Map<IEnumerable<ShipmentDto>>(shipments);

                if (shipments != null && shipments.Any())
                {
                    response.StatusCode = 200;
                    response.IsSuccess = true;
                    response.Result = new ResultDto
                    {
                        Data = mappedShipments
                    };
                    return response;
                }
                else
                {
                    response.Message = "No shipments found";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch
            {
                throw;
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
                    response.Message = "Shipment updated successfully";
                    response.IsSuccess = true;
                    return response;
                }
                response.Message = "Shipment update failed";
                response.IsSuccess = false;
                return response;
            }
            catch
            {
                throw;
            }
        }
    }
}