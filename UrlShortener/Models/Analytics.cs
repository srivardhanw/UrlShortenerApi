namespace UrlShortener.Models
{
    public class Analytics
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int DeviceTypeId { get; set; }
        public DateTime ClickedAt { get; set; }
        public int UrlId { get; set; }
    }
}
