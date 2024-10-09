using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.BatchDtos
{
    public class BatchDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid ProductId { get; set; }
    }
    public class BatchProfile : Profile
    {
        public BatchProfile()
        {
            CreateMap<Batch, BatchDto>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}