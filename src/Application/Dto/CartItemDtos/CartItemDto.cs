using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public Guid ProductId { get; set; }
        public Guid BatchId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool IsBatch { get; set; }
        public ProductDto? Product { get; set; }
        public BatchDto? Batch { get; set; }
    }
    public class CartItemProfile : Profile
    {
        public CartItemProfile()
        {
            CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Batch, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
