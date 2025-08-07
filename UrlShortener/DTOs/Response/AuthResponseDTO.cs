using Microsoft.AspNetCore.Identity;

namespace UrlShortener.DTOs.Response
{
    public class AuthResponseDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;

    }
}
