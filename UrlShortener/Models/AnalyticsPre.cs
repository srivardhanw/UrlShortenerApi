namespace UrlShortener.Models
{
    public class AnalyticsPre
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string DeviceType { get; set; }
        public DateTime ClickedAt { get; set; }
        public int UrlId { get; set; }
    }
}
