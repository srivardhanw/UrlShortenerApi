using System.ComponentModel.DataAnnotations;

namespace UrlShortener.DTOs.Request
{
    public class OriginalRequestDTO
    {
        [Required]
        [RegularExpression("^https:\\/\\/[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}(\\/[^\\s]*)?$", ErrorMessage = "Invalid Url, Please Enter a valid Url")]
        public string OrginalUrl { get; set; } = string.Empty;

    }
}
