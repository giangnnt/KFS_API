using AutoMapper;
using KFS.src.Application.Enum;
using KFS.src.Domain.Entities;
using System.Text.Json.Serialization;

namespace KFS.src.Application.Dto.CategoryDtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<ProductDto1> Products { get; set; } = new List<ProductDto1>();
    }

    public class ProductDto1
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Origin { get; set; }
        public int? Age { get; set; }
        public float? Length { get; set; }
        public string? Species { get; set; }
        public string? Color { get; set; }
        public string? FeedingVolumn { get; set; }
        public float? FilterRate { get; set; }
        public GenderEnum? Gender { get; set; }
        public int Inventory { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ConsignmentId { get; set; }
        public bool IsForSell { get; set; }
    }
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<Product, ProductDto1>();
        }
    }
}
