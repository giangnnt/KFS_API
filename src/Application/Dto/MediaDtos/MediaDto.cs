using AutoMapper;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.MediaDtos
{
    public class MediaDto
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public MediaTypeEnum Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
    public class MediaProfile : Profile
    {
        public MediaProfile()
        {
            CreateMap<Media, MediaDto>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

           

        }
    }
}
