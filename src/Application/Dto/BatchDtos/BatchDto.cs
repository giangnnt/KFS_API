using AutoMapper;
using KFS.src.Application.Dto.PromotionDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.BatchDtos
{
    public class BatchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public int Inventory { get; set; }
        public int Weight { get; set; }
        public decimal Price { get; set; }
        public Guid ProductId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductStatusEnum Status { get; set; }
        public bool IsForSell { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<PromotionDto>? Promotions { get; set; }
    }
    public class BatchProfile : Profile
    {
        public BatchProfile()
        {
            CreateMap<Batch, BatchDto>()
            .ForMember(dest => dest.Promotions, opt => opt.MapFrom(src => src.Promotions))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<BatchUpdate, Batch>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Promotions, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<BatchCreate, Batch>()
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Promotions, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}