using AuthExample.Models;
using UrlShortener.DTOs.Request;
using UrlShortener.DTOs.Response;
using UrlShortener.RepositoryContracts;
using UrlShortener.ServiceContracts;
using UrlShortener.Utilities;

namespace UrlShortener.Services
{
    public class ShortenService : IShortenService
    {
        private readonly ICreationRepository _creationRepository;
        private readonly IAnalyticsService _analyticsService;
        private readonly IGeolocationService _geolocationService;
        private readonly IServiceProvider _serviceProvider;
        public ShortenService(ICreationRepository creationRepository, IAnalyticsService analyticsService, IGeolocationService geolocationService, IServiceProvider serviceProvider)
        {
            _creationRepository = creationRepository;
            _analyticsService = analyticsService;
            _geolocationService = geolocationService;
            _serviceProvider = serviceProvider;
        }

        public async Task<ShortResponseDTO> GetShortUrl(int UserId, OriginalRequestDTO req)
        {
            
            string shortId = ShortIdGenerator.GenerateShortId().Trim();
            
            var res = new ShortResponseDTO
            {
                UserId = UserId,
                OriginalUrl = req.OrginalUrl,
                ShortId = shortId
            };
            Creation creation = new Creation
            {
                OriginalUrl = req.OrginalUrl,
                ShortId = shortId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = UserId,
            };
            int rowId = await _creationRepository.InsertNewCreation(creation);
            if (Convert.ToBoolean(rowId))
            {
                return res;
            }
            return null;
            
        }

        public async Task<OriginalResponseDTO> GetLongUrl(string shortReq, string deviceType, string ip)
        {
            string shortId = shortReq.Trim();
            var creation = await _creationRepository.GetOriginalUrlFromShortId(shortId);
            if(creation != null)
            {
                _ = Task.Run(async () =>
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var geoService = scope.ServiceProvider.GetRequiredService<IGeolocationService>();
                        var analyticsService = scope.ServiceProvider.GetRequiredService<IAnalyticsService>();
                        var geolocation = await geoService.GetGeolocationAsync(ip);
                        analyticsService.InsertAnalytics(creation.Id, deviceType, geolocation);
                    }
                });
                
            }
            var res = new OriginalResponseDTO
            {
                OriginalUrl = creation.OriginalUrl,
            };
            return res;
        }

        
    }
}
