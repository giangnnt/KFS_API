using AutoMapper;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.CategoryDtos
{
    public class categorydtov2
    {
        
           
            public string Name { get; set; } = null!;
            public string? Description { get; set; }
        }
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<categorydtov2, Category>();
          
          
            
           
        }
    }

}

