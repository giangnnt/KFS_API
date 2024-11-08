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
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.DistrictId, opt => opt.MapFrom(src => src.DistrictId))
            .ForMember(dest => dest.WardCode, opt => opt.MapFrom(src => src.WardCode))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.PhysicsAddress))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}