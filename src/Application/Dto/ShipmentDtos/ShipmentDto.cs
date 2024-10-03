using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.ShipmentDtos
{
    public class ShipmentDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ShipmentStatusEnum Status { get; set; }
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