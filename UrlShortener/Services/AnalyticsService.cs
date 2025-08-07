using Azure.Core.GeoJson;
using UrlShortener.DTOs.Response;
using UrlShortener.Models;
using UrlShortener.RepositoryContracts;
using UrlShortener.ServiceContracts;

namespace UrlShortener.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public AnalyticsService(IAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public void InsertAnalytics(int UrlId, string deviceType, GeolocationDTO geolocation)
        {
            AnalyticsPre analytics = new AnalyticsPre
            {
                Country = geolocation.Country,
                City = geolocation.City,
                DeviceType = deviceType,
                ClickedAt = DateTime.UtcNow,
                UrlId = UrlId
            };
            _analyticsRepository.AddNewAnalytics(analytics);
            
        }
    }
}
