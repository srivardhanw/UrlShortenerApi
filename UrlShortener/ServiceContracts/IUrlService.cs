using UrlShortener.DTOs.Request;
using UrlShortener.DTOs.Response;

namespace UrlShortener.ServiceContracts
{
    public interface IUrlService
    {
        public Task<ShortResponseDTO> GetShortUrl(int UserId, OriginalRequestDTO req);

        public Task<OriginalResponseDTO> GetLongUrl(string shortReq, string deviceType, string ip);

        public Task<List<MyLinksResponseDTO>> GetUrlsByUserId(int userId);

        public Task<bool> IsUrlOwnedByUser(int userId, int urlId);

        

    }
}
