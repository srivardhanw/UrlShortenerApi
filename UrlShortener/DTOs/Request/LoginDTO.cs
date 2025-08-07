using System.ComponentModel.DataAnnotations;

namespace UrlShortener.DTOs.Request
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username or Email is required")]
        public string EmailOrUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
