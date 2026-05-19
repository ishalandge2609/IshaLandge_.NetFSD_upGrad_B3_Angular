using System.ComponentModel.DataAnnotations;

namespace OrderService.API.DTOs
{
    public class CreateOrderItemDTO
    {
        // Product to be added in the order
        [Required, Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        // Quantity of the selected product
        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class CreateOrderDTO
    {
        // List of products included in the order
        [Required, MinLength(1)]
        public List<CreateOrderItemDTO> Items { get; set; } = new();
    }
}