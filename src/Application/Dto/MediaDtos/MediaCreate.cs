using AutoMapper;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.MediaDtos
{
    public class MediaCreate
    {
      
        public string? Url { get; set; }
        public Guid? ProductId { get; set; }
    }
    public class mediaprofile : Profile
    {
        public mediaprofile()
        {
            CreateMap<MediaCreate, Media>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); ;
                

            

        }
    }
}
