using AutoMapper;
using KFS.src.Application.Dto.CategoryDtos;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.MediaDtos
{
    public class MediaDto
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public Guid? ProductId { get; set; }
       
       
       
    }
    public class MediaProfile : Profile
    {
        public MediaProfile()
        {
            CreateMap<Media, MediaDto>();

           

        }
    }
}
