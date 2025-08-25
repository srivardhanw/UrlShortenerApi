namespace UrlShortener.Models
{
    public class DeviceComposition
    {
        
        public string TypeName { get; set; } = string.Empty; // desktop, mobile, tablet, e-reader, unknown
        public int TotalClicksByDevice { get; set; }

    }
}
