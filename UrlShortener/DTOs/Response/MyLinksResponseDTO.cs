namespace UrlShortener.DTOs.Response
{
    public class MyLinksResponseDTO
    {
        public int UrlId { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
