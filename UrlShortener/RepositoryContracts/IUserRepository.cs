using AuthExample.Models;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DTOs.Request;

namespace UrlShortener.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<bool> IsUserExistsAsync(string username);
        Task<int> InsertNewUserAsync(User user);
        Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);

    }
}
