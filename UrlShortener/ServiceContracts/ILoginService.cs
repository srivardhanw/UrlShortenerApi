using AuthExample.Models;
using UrlShortener.DTOs.Response;

namespace UrlShortener.ServiceContracts
{
    public interface ILoginService
    {
        Task<AuthResponseDTO?> AuthenticateUserByUsernameOrEmailAndPassword(string username, string password);
    }
}
