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
                .Include<CartItemProduct, OrderItemProduct>()
                .Include<CartItemBatch, OrderItemBatch>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<OrderItem, OrderItemDto>()
                .Include<OrderItemProduct, OrderItemDto>()
                .Include<OrderItemBatch, OrderItemDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<OrderItemProduct, OrderItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<OrderItemBatch, OrderItemDto>()
                .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.BatchId))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CartItemProduct, OrderItemProduct>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CartItemBatch, OrderItemBatch>()
                .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.BatchId))
                .ForMember(dest => dest.Batch, opt => opt.MapFrom(src => src.Batch))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}