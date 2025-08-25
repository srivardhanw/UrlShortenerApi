using System.ComponentModel.DataAnnotations;

namespace UrlShortener.DTOs.Request
{
    public class DonutRequestDTO
    {
        [Required]
        public int UrlId {  get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
