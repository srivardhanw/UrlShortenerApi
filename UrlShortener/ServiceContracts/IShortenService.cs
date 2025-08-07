using UrlShortener.DTOs.Request;
using UrlShortener.DTOs.Response;

namespace UrlShortener.ServiceContracts
{
    public interface IShortenService
    {
        public Task<ShortResponseDTO> GetShortUrl(int UserId, OriginalRequestDTO req);

        public Task<OriginalResponseDTO> GetLongUrl(string shortReq, string deviceType, string ip);
       
    }
}
