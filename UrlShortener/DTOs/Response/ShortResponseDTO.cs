using System.ComponentModel.DataAnnotations;

namespace UrlShortener.DTOs.Response
{
    public class ShortResponseDTO
    {
        public int UserId { get; set; }
        [Required]
        public string OriginalUrl { get; set; } = string.Empty;
        [Required]
        public string ShortId { get; set; }  = string.Empty;

    }
}
