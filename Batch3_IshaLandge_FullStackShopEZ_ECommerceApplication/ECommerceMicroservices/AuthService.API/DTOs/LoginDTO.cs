using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs
{
    public class LoginDTO
    {
        // User email for login
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        // User password for authentication
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}