using Azure.Core.GeoJson;
using System.Runtime.InteropServices;
using UrlShortener.DTOs.Request;
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
            Analytics analytics = new Analytics
            {
                Country = geolocation.Country,
                City = geolocation.City,
                DeviceType = deviceType,
                ClickedAt = DateTime.UtcNow,
                UrlId = UrlId
            };
            _analyticsRepository.AddNewAnalytics(analytics);
            
        }

        public async Task<ClicksResponseDTO> GetClicksByUrlId(int urlId)
        {
            var clicks = await _analyticsRepository.GetClicksByUrlId(urlId);
            ClicksResponseDTO clicksResponse = new ClicksResponseDTO
            {
                UrlId = clicks.UrlId,
                OriginalUrl = clicks.OriginalUrl,
                ShortId = clicks.ShortId,
                Clicks = clicks.Clicks,
            };
            return clicksResponse;
        }

        public async Task<SeriesResponseDTO> GetLineByUrlId(SeriesRequestDTO seriesRequest)
        {
            var series = await _analyticsRepository.GetLineByUrlId(seriesRequest.UrlId, seriesRequest.StartDate, seriesRequest.EndDate, seriesRequest.GroupBy);

            SeriesResponseDTO res = new SeriesResponseDTO
            {
                GroupedBy = seriesRequest.GroupBy.ToString(),
                Series = series.Select(s => new SeriesUnit
                {
                    StartDate = s.StartDate,
                    TotalClicks = s.TotalClicks,
                }).ToList(),
            };

            return res;
        }
    }
}
