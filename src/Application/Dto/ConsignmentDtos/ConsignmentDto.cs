using AutoMapper;
using KFS.src.Application.Dto.BatchDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.ConsignmentDtos
{
    public class ConsignmentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConsignmentMethodEnum Method { get; set; }
        public int CommissionPercentage { get; set; }
        public int DealingAmount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConsignmentStatusEnum Status { get; set; }
        public bool IsForSell { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ConsignmentFee { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsBatch { get; set; }
        public ProductDtoNoBatch? Product { get; set; } = new();
        public BatchDto? Batch { get; set; } = new();
    }
    public class ConsignmentProfile : Profile
    {
        public ConsignmentProfile()
        {
            CreateMap<Consignment, ConsignmentDto>()
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}