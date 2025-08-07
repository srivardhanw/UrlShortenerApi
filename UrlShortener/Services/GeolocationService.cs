using Newtonsoft.Json;
using UrlShortener.DTOs.Response;
using UrlShortener.ServiceContracts;

namespace UrlShortener.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GeolocationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GeolocationDTO> GetGeolocationAsync(string ip)
        {
            string url = $"http://ip-api.com/json/{ip}";
            var client = _httpClientFactory.CreateClient();
            var res = await client.GetAsync($"{url}?fields=status,country,city");

            if (!res.IsSuccessStatusCode) throw new Exception("Failed to fetch data from ip-api.com");

            var content = await res.Content.ReadAsStringAsync();

            var geolocation = JsonConvert.DeserializeObject<GeolocationDTO>(content);

            return geolocation;
        }
    }
}
