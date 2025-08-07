using AuthExample.Models;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.DTOs.Response;
using UrlShortener.RepositoryContracts;
using UrlShortener.ServiceContracts;
using UrlShortener.Utilities;

namespace UrlShortener.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public LoginService(IUserRepository userRepository, IConfiguration configuration) {
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public async Task<AuthResponseDTO?> AuthenticateUserByUsernameOrEmailAndPassword(string emailOrUsername, string password)
        {
           
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(emailOrUsername); // get user
            if (user == null)
            {
                throw new Exception("Invalid Request");
            }
            bool isValid = PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);

            if (!isValid)
            {
                throw new Exception("Invalid Request");
            }


            string? secret = _configuration["JwtSettings:Key"];
            if (secret.IsNullOrEmpty())
            {
                throw new Exception("JWT secret is missing from configuration");
            }
            string JwtString = JwtHelper.GenerateJwtToken(user, secret!, 25);
            AuthResponseDTO sessionToken = new AuthResponseDTO
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.Username,
                Token = JwtString,
            };
            
            return sessionToken;
            
        }
    }
}
