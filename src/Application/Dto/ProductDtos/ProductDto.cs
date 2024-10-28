using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.FeedbackDtos;
using KFS.src.Application.Dto.MediaDtos;
using KFS.src.Application.Dto.PromotionDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.ProductDtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Price { get; set; }
        public string? Origin { get; set; }
        public int? Age { get; set; }
        public int Weight { get; set; }
        public float? Length { get; set; }
        public string? Species { get; set; }
        public string? Color { get; set; }
        public string? FeedingVolumn { get; set; }
        public float? FilterRate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderEnum Gender { get; set; }
        public int Inventory { get; set; }
        public bool IsForSell { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ConsignmentId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<BatchDto>? Batches { get; set; }
        public List<PromotionDto>? Promotions { get; set; }
        public List<MediaDto>? Medias { get; set; }
        public List<FeedbackDto>? Feedbacks { get; set; }
    }
    public class ProductDtoNoBatch
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal? Price { get; set; }
        public string? Origin { get; set; }
        public int? Age { get; set; }
        public int Weight { get; set; }
        public float? Length { get; set; }
        public string? Species { get; set; }
        public string? Color { get; set; }
        public string? FeedingVolumn { get; set; }
        public float? FilterRate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderEnum Gender { get; set; }
        public int Inventory { get; set; }
        public bool IsForSell { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ConsignmentId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PromotionDto>? Promotions { get; set; }
        public List<MediaDto>? Medias { get; set; }
        public List<FeedbackDto>? Feedbacks { get; set; }
    }
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Batches, opt => opt.MapFrom(src => src.Batches))
            .ForMember(dest => dest.Promotions, opt => opt.MapFrom(src => src.Promotions))
            .ForMember(dest => dest.Medias, opt => opt.MapFrom(src => src.Medias))
            .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Feedbacks))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ProductCreate, Product>()
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Gender, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ProductUpdate, Product>()
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.CartItems, opt => opt.Ignore())
            .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Product, ProductDtoNoBatch>()
            .ForMember(dest => dest.Promotions, opt => opt.MapFrom(src => src.Promotions))
            .ForMember(dest => dest.Medias, opt => opt.MapFrom(src => src.Medias))
            .ForMember(dest => dest.Feedbacks, opt => opt.MapFrom(src => src.Feedbacks))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}