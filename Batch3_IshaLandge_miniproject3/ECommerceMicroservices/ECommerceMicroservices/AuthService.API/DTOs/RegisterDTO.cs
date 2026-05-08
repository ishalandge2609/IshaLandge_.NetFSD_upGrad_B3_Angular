using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least 1 uppercase letter and 1 number")]
        public string Password { get; set; } = string.Empty;
    }
}