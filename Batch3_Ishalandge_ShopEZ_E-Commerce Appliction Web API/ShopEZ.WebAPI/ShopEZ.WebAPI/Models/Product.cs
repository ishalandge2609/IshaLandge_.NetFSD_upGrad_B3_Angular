using System.ComponentModel.DataAnnotations;

namespace ShopEZ.WebAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int Stock { get; set; }

        // Navigation property :
       
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
