namespace AuthService.API.DTOs
{
    public class LoginResponseDTO
    {
        // JWT token generated after successful login
        public string Token { get; set; } = string.Empty;

        // Logged-in user's email
        public string Email { get; set; } = string.Empty;

        // Logged-in user's role
        public string Role { get; set; } = string.Empty;

        // Stores error message in case login fails
        public string Error { get; set; }
    }
}