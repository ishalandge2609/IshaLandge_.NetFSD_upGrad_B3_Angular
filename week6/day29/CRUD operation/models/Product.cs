using System.ComponentModel.DataAnnotations;

namespace MyFirstMVCApp.Models
{
    public class Product
    {
            
            [Required]
            public int ProductId { get; set; }

            [Required]
            [StringLength(15, MinimumLength = 5)]
            public string ProductName { get; set; }

            [Required]
            public double Price { get; set; }

            [StringLength(15, MinimumLength = 5)]
            public string Category { get; set; }
        }
    }


