using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.AddressDtos
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public string PhysicsAddress { get; set; } = null!;
        public string WardCode { get; set; } = null!;
        public int DistrictId { get; set; }
        public Guid UserId { get; set; }
    }
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AddressCreate, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}