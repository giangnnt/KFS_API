using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.PromotionDtos
{
    public class PromotionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? PromotionCode { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<ProductDto> Products { get; set; } = new();
        public List<BatchDto> Batches { get; set; } = new();
        public List<CategotyDto> Categories { get; set; } = new();
    }
    public class PromotionProfile : Profile
    {
        public PromotionProfile()
        {
            CreateMap<Promotion, PromotionDto>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
            .ForMember(dest => dest.Batches, opt => opt.MapFrom(src => src.Batches))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PromotionCreate, Promotion>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.Batches, opt => opt.Ignore())
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PromotionUpdate, Promotion>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.Batches, opt => opt.Ignore())
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}