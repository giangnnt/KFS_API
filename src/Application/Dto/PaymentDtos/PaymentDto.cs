using AutoMapper;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.PaymentDtos
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ConsignmentId { get; set; }
        public Guid UserId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatusEnum Status { get; set; }
        public string Currency { get; set; } = "VND";
        public string TransactionId { get; set; } = null!;
        public string PaymentType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>()
                .Include<PaymentOrder, PaymentDto>()
                .Include<PaymentConsignment, PaymentDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PaymentOrder, PaymentDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PaymentConsignment, PaymentDto>()
                .ForMember(dest => dest.ConsignmentId, opt => opt.MapFrom(src => src.ConsignmentId))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}