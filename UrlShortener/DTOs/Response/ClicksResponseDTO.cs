namespace UrlShortener.DTOs.Response
{
    public class ClicksResponseDTO
    {
        public int UrlId { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortId { get; set; } = string.Empty;
        public int Clicks { get; set; } = 0;
    }
}
