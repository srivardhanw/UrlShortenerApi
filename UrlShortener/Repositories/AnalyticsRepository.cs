using Dapper;
using System.Data;
using UrlShortener.Models;
using UrlShortener.RepositoryContracts;

namespace UrlShortener.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly IDbConnection _dbConnection;

        public AnalyticsRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public void AddNewAnalytics(Analytics analytics)
        {
            var query = @"INSERT INTO Analytics (CountryId, CityId, DeviceTypeId, ClickedAt, UrlId)
                        VALUES ((SELECT CountryId FROM CountryMaster WHERE CountryMaster.CountryName = @Country), 
                                (SELECT CityId FROM CityMaster WHERE CityMaster.CityName = @City), 
                                (SELECT TypeId FROM DeviceTypeMaster WHERE DeviceTypeMaster.TypeName = @DeviceType),
                                @ClickedAt, @UrlId)";
            

            _dbConnection.Execute(query, new
            {
                Country = analytics.Country,
                City = analytics.City,
                DeviceType = analytics.DeviceType,
                ClickedAt = analytics.ClickedAt,
                UrlId = analytics.UrlId
            });



        }
    }
}
