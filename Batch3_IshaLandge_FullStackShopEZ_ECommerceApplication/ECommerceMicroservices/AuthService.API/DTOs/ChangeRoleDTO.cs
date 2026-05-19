using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs
{
    public class ChangeRoleDTO
    {
        // Stores the new role to be assigned to the user
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}