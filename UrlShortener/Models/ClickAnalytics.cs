namespace UrlShortener.Models
{
    public class ClickAnalytics
    {
        public int UrlId { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortId { get; set; } = string.Empty;
        public int Clicks { get; set; } = 0;
    }
}
