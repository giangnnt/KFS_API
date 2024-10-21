using AutoMapper;
using KFS.src.Application.Dto.OrderDtos;
using KFS.src.Domain.Entities;

namespace KFS.src.Application.Dto.FeedbackDtos
{
    public class FeedbackCreate
    {
        public string? Description { get; set; }
        public int Rating { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        
   

    }
    
          


}
