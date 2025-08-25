using System.ComponentModel.DataAnnotations;
using UrlShortener.Enums;

namespace UrlShortener.DTOs.Request
{
    public class SeriesRequestDTO
    {
        [Required]
        public int UrlId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public GroupBy GroupBy { get; set; }
    }
}
