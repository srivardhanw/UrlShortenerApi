using UrlShortener.Models;

namespace UrlShortener.RepositoryContracts
{
    public interface IAnalyticsRepository
    {
        public void AddNewAnalytics(Analytics analyticsPre);
    }
}
