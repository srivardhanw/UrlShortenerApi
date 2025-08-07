using UrlShortener.DTOs.Response;

namespace UrlShortener.ServiceContracts
{
    public interface IGeolocationService
    {
        public Task<GeolocationDTO> GetGeolocationAsync(string ip);
    }
}
