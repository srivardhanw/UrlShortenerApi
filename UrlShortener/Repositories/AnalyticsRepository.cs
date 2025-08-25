using Dapper;
using System.Data;
using UrlShortener.DTOs.Response;
using UrlShortener.Enums;
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

        public async Task<ClickAnalytics> GetClicksByUrlId(int urlId)
        {
            var query = @"SELECT c.Id AS UrlId, OriginalUrl,ShortId,  COUNT(a.UrlId) AS Clicks 
                        FROM Creation AS c
                        LEFT JOIN Analytics AS a 
                        ON c.Id = a.UrlId
                        WHERE c.id = @UrlId
                        GROUP BY c.id,OriginalUrl,ShortId";

            var data = await _dbConnection.QueryFirstOrDefaultAsync<ClickAnalytics>(query, new {UrlId = urlId});
            
            return data;
        }

        public async Task<List<Series>> GetLineByUrlId(int UrlId, DateTime StartDate, DateTime EndDate, GroupBy GroupBy)
        {
            string groupBySql = GroupBy switch
            {
                GroupBy.day => "CAST(a.ClickedAt AS DATE)",
                GroupBy.week => "CAST(DATETRUNC(week, a.ClickedAt) AS DATE)",
                GroupBy.month => "FORMAT(a.ClickedAt, 'yyyy-MM-01')", 
                GroupBy.year => "FORMAT(a.ClickedAt, 'yyyy-01-01')",
                _ => throw new ArgumentException("Invalid GroupBy")
            };

            var query = $@"SELECT
                            {groupBySql} AS StartDate,
                            COUNT(*) AS TotalClicks
                        FROM Analytics AS a
                        WHERE a.UrlId = @UrlId
                          AND a.ClickedAt >= @StartDate
                          AND a.ClickedAt <  @EndDate
                        GROUP BY {groupBySql}
                        ORDER BY StartDate;";
            var series = await _dbConnection.QueryAsync<Series>(query, new { UrlId = UrlId, StartDate = StartDate, EndDate = EndDate, });
            return series.ToList();
        }

        public async Task<Donut> GetDonutByUrlId(int UrlId, DateTime StartDate, DateTime EndDate)
        {
            var TotalCountQuery = @" SELECT COUNT(*) AS TotalClicks FROM Analytics AS a
                                    WHERE a.UrlId = @UrlId AND 
                                    a.ClickedAt >= @StartDate AND 
                                    a.ClickedAt <  @EndDate";

            var deviceCompositionQuery = @"SELECT dtm.TypeName, COUNT(*) AS TotalClicksByDevice FROM Analytics AS a
                        JOIN DeviceTypeMaster AS dtm ON a.DeviceTypeId = dtm.TypeId
                        WHERE a.UrlId = @UrlId 
	                        AND a.ClickedAt >= @StartDate 
	                        AND a.ClickedAt <  @EndDate
                        GROUP BY dtm.TypeName";
            Donut donut = new Donut();
            donut.TotalClicks = await _dbConnection.QuerySingleAsync<int>(TotalCountQuery, new { UrlId = UrlId, StartDate = StartDate, EndDate = EndDate });
            donut.DeviceComposition = (await _dbConnection.QueryAsync<DeviceComposition>(deviceCompositionQuery, new { UrlId = UrlId, StartDate = StartDate, EndDate = EndDate })).ToList();
            return donut;
        }
    }
}
