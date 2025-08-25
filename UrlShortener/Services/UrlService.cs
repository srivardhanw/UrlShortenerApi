using AuthExample.Models;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;
using UrlShortener.BackgroundServices;
using UrlShortener.DTOs.Request;
using UrlShortener.DTOs.Response;
using UrlShortener.RepositoryContracts;
using UrlShortener.ServiceContracts;
using UrlShortener.Utilities;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly ICreationRepository _creationRepository;
        private readonly IAnalyticsService _analyticsService;
        private readonly IGeolocationService _geolocationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBackgroundAnalyticsQueue _backgroundAnalyticsQueue;
        public UrlService(ICreationRepository creationRepository, IAnalyticsService analyticsService, IGeolocationService geolocationService, IServiceProvider serviceProvider, IBackgroundAnalyticsQueue backgroundAnalyticsQueue)
        {
            _creationRepository = creationRepository;
            _analyticsService = analyticsService;
            _geolocationService = geolocationService;
            _serviceProvider = serviceProvider;
            _backgroundAnalyticsQueue = backgroundAnalyticsQueue;
        }

        public async Task<ShortResponseDTO> GetShortUrl(int UserId, OriginalRequestDTO req)
        {
            
            string shortId = ShortIdGenerator.GenerateShortId().Trim();
            // should implement a collision check loop
            
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
                await _backgroundAnalyticsQueue.EnqueueAsync(async (sp, ct) =>
                {
                    var geoService = sp.GetRequiredService<IGeolocationService>();
                    var analyticsService = sp.GetRequiredService<IAnalyticsService>();

                    var geolocation = await geoService.GetGeolocationAsync(ip);

                    analyticsService.InsertAnalytics(creation.Id, deviceType, geolocation);
                });

            }
            var res = new OriginalResponseDTO
            {
                OriginalUrl = creation.OriginalUrl,
            };
            return res;
        }

        public async Task<List<MyLinksResponseDTO>> GetUrlsByUserId(int userId)
        {
            // call userRepo.GetUrlsById(userId)
            var urls = await _creationRepository.GetUrlsByUserId(userId);
            List<MyLinksResponseDTO> myLinksResponseDTOs = urls.Select(c => new MyLinksResponseDTO
            {
                UrlId = c.Id,
                OriginalUrl = c.OriginalUrl,
                ShortId = c.ShortId,
                CreatedAt = c.CreatedAt
            }).ToList();

            return myLinksResponseDTOs;
        }

        public async Task<bool> IsUrlOwnedByUser(int userId, int urlId)
        {
            bool isOwnedByUser = await _creationRepository.IsUrlOwnedByUser(userId, urlId);
            return isOwnedByUser;
        }

      
    }
}
