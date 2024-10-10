using AutoMapper;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.CategoryDtos
{
    public class categoryv3
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public  Guid? CategoryId { get; private set; }
    }
    public class CategoryProfilee : Profile
    {
        public CategoryProfilee()
        {
            CreateMap<categoryv3, Category>();




        }
    }
}

