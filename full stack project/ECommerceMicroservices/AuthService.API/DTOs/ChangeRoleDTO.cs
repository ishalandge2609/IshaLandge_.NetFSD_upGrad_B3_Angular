using System.ComponentModel.DataAnnotations;

namespace AuthService.API.DTOs
{
    public class ChangeRoleDTO
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}