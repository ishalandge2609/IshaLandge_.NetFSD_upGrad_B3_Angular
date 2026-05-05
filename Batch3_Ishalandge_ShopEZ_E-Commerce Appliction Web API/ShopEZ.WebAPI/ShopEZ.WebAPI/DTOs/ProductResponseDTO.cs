using System.ComponentModel.DataAnnotations;

namespace ShopEZ.WebAPI.DTOs
{
    public class ProductResponseDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

     
        public string Description { get; set; }

        
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int Stock { get; set; }
    }
}
