using UrlShortener.Enums;

namespace UrlShortener.DTOs.Response
{
    public class SeriesResponseDTO
    {
        public string GroupedBy { get; set; } = string.Empty;
        public List<SeriesUnit> Series {  get; set; } 

    }
}
