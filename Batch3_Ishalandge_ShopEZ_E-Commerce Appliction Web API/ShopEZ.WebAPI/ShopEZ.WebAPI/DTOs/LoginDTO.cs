using System.ComponentModel.DataAnnotations;

namespace ShopEZ.WebAPI.DTOs
{
    public class LoginDTO
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
