using Microsoft.AspNetCore.Mvc;
using UrlShortener.DTOs.Request;
using UrlShortener.Enums;

namespace UrlShortener.ServiceContracts
{
    public interface IRegisterService
    {
        Task<RegisterResult> RegisterUser(RegisterDTO registerDTO);
    }
}
