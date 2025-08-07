using UrlShortener.DTOs.Response;

namespace UrlShortener.ServiceContracts
{
    public interface IAnalyticsService
    {
        public void InsertAnalytics(int UrlId, string deviceType, GeolocationDTO geolocation);
    }
}
