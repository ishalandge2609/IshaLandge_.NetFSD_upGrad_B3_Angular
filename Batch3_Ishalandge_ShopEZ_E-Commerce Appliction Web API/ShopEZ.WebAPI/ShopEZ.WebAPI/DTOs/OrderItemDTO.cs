using System.ComponentModel.DataAnnotations;

namespace ShopEZ.WebAPI.DTOs
{
    public class OrderItemDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
    
}
