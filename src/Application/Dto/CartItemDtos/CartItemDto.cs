using AutoMapper;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.CartItemDtos
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public decimal Price { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? BatchId { get; set; }
    }
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemDto>()
                .Include<CartItemProduct, CartItemDto>()
                .Include<CartItemBatch, CartItemDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CartItemProduct, CartItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CartItemBatch, CartItemDto>()
                .ForMember(dest => dest.BatchId, opt => opt.MapFrom(src => src.BatchId))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
