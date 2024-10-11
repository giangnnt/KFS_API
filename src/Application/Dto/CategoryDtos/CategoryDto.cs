using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Application.Dto.ProductDtos;
using KFS.src.Application.Dto.PromotionDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.CategoryDtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public List<PromotionDto>? Promotions { get; set; }
        public List<ProductDto> Products { get; set; } = new();
    }
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}