using System.ComponentModel.DataAnnotations;

namespace UrlShortener.DTOs.Request
{
    public class ShortRequestDTO
    {
        [Required]
        public string ShortId { get; set; } = string.Empty;
       
    }
}
