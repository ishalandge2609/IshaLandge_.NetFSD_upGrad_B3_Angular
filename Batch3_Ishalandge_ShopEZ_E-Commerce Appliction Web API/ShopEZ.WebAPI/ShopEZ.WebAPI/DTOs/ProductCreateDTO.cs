using System.ComponentModel.DataAnnotations;

namespace ShopEZ.WebAPI.DTOs
{
    public class ProductCreateDTO
    {
        [Required]
        [MinLength(4,
        ErrorMessage = " Pease enter Product Name ")]
        public string Name { get; set; }

        [Required]
        [MinLength(10,
        ErrorMessage = " Enter valid  description for the product ")]
        public string Description { get; set; }

        [Required]
        [Range(1, 1000000)]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Stock { get; set; }
    }
}
