using System.ComponentModel.DataAnnotations;

namespace AuthExample.Models
{
    public class Creation
    {
        public int Id { get; set; }
        [Required]
        public string OriginalUrl { get; set; } = string.Empty;
        [Required]
        public string ShortId {  get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; }

        public int CreatedBy  { get; set; }

    }
}
