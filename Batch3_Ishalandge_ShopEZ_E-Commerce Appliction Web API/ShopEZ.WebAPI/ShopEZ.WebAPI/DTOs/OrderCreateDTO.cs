using System.ComponentModel.DataAnnotations;

namespace ShopEZ.WebAPI.DTOs
{
    public class OrderCreateDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Order must have at least one item")]
        public List<OrderItemDTO> Items { get; set; }
    }
}
