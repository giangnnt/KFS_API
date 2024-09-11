using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.CartItemDtos;
using KFS.src.Application.Dto.UserDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.CartDtos
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalItem { get; set; }
        public string Currency { get; set; } = "VND";
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CartStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserDto User { get; set; } = null!;
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
    }
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CartCreate, Cart>()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.CartItems, opt => opt.Ignore());
            CreateMap<CartUpdate, Cart>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}