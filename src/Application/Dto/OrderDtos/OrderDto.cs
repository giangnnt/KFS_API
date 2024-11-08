using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.AddressDtos;
using KFS.src.Application.Dto.OrderItemDtos;
using KFS.src.Application.Dto.PaymentDtos;
using KFS.src.Application.Dto.ShipmentDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.OrderDtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AddressId { get; set; }
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public int TotalWeight { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalItem { get; set; }
        public string ContactNumber { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public int ShippingFee { get; set; }
        public float Discount { get; set; }
        public bool IsUsePoint { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string? EstimatedDeliveryDate { get; set; }
        public string? Note { get; set; }
        public string Currency { get; set; } = "VND";
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum Status { get; set; }
        public bool IsReBuy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public PaymentDto? Payment { get; set; }
        public ShipmentDto? Shipment { get; set; }
        public AddressDto? Address { get; set; }
    }
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment))
            .ForMember(dest => dest.Shipment, opt => opt.MapFrom(src => src.Shipment))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<OrderCreateFromCart, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
            .ForMember(dest => dest.Payment, opt => opt.Ignore())
            .ForMember(dest => dest.TotalItem, opt => opt.Ignore())
            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
            .ForMember(dest => dest.TotalWeight, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Cart, Order>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.TotalItem, opt => opt.MapFrom(src => src.TotalItem))
            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.CartItems))
            .ForMember(dest => dest.TotalWeight, opt => opt.MapFrom(src => src.TotalWeight))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}