using UrlShortener.DTOs.Request;
using UrlShortener.DTOs.Response;
using UrlShortener.Enums;
using UrlShortener.Models;

namespace UrlShortener.RepositoryContracts
{
    public interface IAnalyticsRepository
    {
        public void AddNewAnalytics(Analytics analyticsPre);

        public Task<ClickAnalytics> GetClicksByUrlId(int urlId);
        public Task<List<Series>> GetLineByUrlId(int UrlId, DateTime StartDate, DateTime EndDate, GroupBy GroupBy);

        public Task<Donut> GetDonutByUrlId(int UrlId, DateTime StartDate, DateTime EndDate);

    }
}
