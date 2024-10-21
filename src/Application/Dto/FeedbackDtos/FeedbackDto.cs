using AutoMapper;
using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.FeedbackDtos
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public int Rating { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }

    }
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<FeedbackCreate, Feedback>()


            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<FeedbackUpdate, Feedback>()


           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
          
            CreateMap<Feedback, FeedbackDto>()


           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}