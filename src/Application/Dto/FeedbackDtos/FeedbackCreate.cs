using System.ComponentModel.DataAnnotations;

namespace KFS.src.Application.Dto.FeedbackDtos
{
    public class FeedbackCreate
    {
        public string? Description { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }
    }
}