using UrlShortener.DTOs.Request;
using UrlShortener.DTOs.Response;
using UrlShortener.Models;

namespace UrlShortener.ServiceContracts
{
    public interface IAnalyticsService
    {
        public void InsertAnalytics(int UrlId, string deviceType, GeolocationDTO geolocation);

        public Task<ClicksResponseDTO> GetClicksByUrlId(int urlId);

        public Task<SeriesResponseDTO> GetLineByUrlId(SeriesRequestDTO seriesRequest);

        public Task<DonutResponseDTO> GetDonutByUrlId(DonutRequestDTO donutRequest);
    }
}
