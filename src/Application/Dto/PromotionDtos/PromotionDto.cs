using AutoMapper;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.PromotionDtos
{
    public class PromotionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? DiscountCode { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class PromotionProfile : Profile
    {
        public PromotionProfile()
        {
            CreateMap<Promotion, PromotionDto>()
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