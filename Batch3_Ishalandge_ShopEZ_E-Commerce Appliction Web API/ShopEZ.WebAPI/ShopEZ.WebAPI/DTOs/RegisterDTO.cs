using System.ComponentModel.DataAnnotations;

namespace ShopEZ.WebAPI.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [MinLength(4, 
        ErrorMessage = "Name must contain at least 4 letters")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
         public string Email { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
        ErrorMessage = "Password must contain at least 1 letter, 1 number, and 1 special character")]
        public string Password { get; set; }
       
        [Required]
        [RegularExpression("^(Admin|User)$", ErrorMessage = "Role must be Admin or User")]
        public string Role { get; set; }
    }
}
