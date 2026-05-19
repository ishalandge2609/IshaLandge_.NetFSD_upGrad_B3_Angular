using System.ComponentModel.DataAnnotations;

namespace ProductService.API.DTOs
{
    public class CreateProductDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required, Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }
        [Required]
        public string Category { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
    }
}