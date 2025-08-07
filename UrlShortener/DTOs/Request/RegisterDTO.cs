using System.ComponentModel.DataAnnotations;

namespace UrlShortener.DTOs.Request
{
    public class RegisterDTO
    {
        [Required]
        [RegularExpression("^[A-Za-z][A-Za-z0-9_]{7,29}$", ErrorMessage = "Username should start with alphabets, length between 8-30 characters, should contain no spaces")]
        public string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
