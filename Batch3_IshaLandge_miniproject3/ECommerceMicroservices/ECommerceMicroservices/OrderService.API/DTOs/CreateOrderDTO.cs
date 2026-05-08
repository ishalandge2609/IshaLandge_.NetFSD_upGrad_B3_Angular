using System.ComponentModel.DataAnnotations;

namespace OrderService.API.DTOs
{
    public class CreateOrderItemDTO
    {
        [Required, Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class CreateOrderDTO
    {
        [Required, MinLength(1)]
        public List<CreateOrderItemDTO> Items { get; set; } = new();
    }
}