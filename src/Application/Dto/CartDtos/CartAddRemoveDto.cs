using System.ComponentModel.DataAnnotations;

namespace KFS.src.Application.Dto.CartDtos
{
    public class CartAddRemoveDto
    {
        public Guid ProductId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
}