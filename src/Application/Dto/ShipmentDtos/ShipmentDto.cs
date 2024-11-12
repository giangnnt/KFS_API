using AutoMapper;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.ShipmentDtos
{
    public class ShipmentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ShipperId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ShipmentStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile()
        {
            CreateMap<Shipment, ShipmentDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}