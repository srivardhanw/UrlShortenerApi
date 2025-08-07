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
        public void AddNewAnalytics(AnalyticsPre analyticsPre)
        {
            var query = @"INSERT INTO Analytics (CountryId, CityId, DeviceTypeId, ClickedAt, UrlId)
                        VALUES ((SELECT CountryId FROM CountryMaster WHERE CountryMaster.CountryName = @Country), 
                                (SELECT CityId FROM CityMaster WHERE CityMaster.CityName = @City), 
                                (SELECT TypeId FROM DeviceTypeMaster WHERE DeviceTypeMaster.TypeName = @DeviceType),
                                @ClickedAt, @UrlId)";
            

            _dbConnection.Execute(query, new
            {
                Country = analyticsPre.Country,
                City = analyticsPre.City,
                DeviceType = analyticsPre.DeviceType,
                ClickedAt = analyticsPre.ClickedAt,
                UrlId = analyticsPre.UrlId
            });


            
        }
    }
}
