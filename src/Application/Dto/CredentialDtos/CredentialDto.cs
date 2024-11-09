using AutoMapper;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.CredentialDtos
{
    public class CredentialDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? CredentialFile { get; set; }
        public Guid ProductId { get; set; }
    }
    public class CredentialProfile : Profile
    {
        public CredentialProfile()
        {
            CreateMap<Credential, CredentialDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<credentialCreate, Credential>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<CredentialUpdate, Credential>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}