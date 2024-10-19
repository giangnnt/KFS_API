using AutoMapper;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.MediaDtos
{
    public class MediaUpdate
    {

        public string? Url { get; set; }
        public MediaTypeEnum Type { get; set; }

        public class MediaProfile : Profile
        {
            public MediaProfile()
            {
                CreateMap<Media, MediaUpdate>();
                CreateMap<MediaUpdate, Media>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }
        }
    }
}
