using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.OrderItemDtos
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsBatch { get; set; }
        public ProductDto? Product { get; set; }
        public BatchDto? Batch { get; set; }
    }
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<CartItem, OrderItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OrderId, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.BatchId))   
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.IsBatch, opt => opt.MapFrom(src => src.IsBatch))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Batch, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}