using AutoMapper;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.WalletDtos
{
    public class WalletDto
    {
        public Guid Id { get; set; }
        public int Point { get; set; }
        public Guid UserId { get; set; }
    }
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<Wallet, WalletDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}