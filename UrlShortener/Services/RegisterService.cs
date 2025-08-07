using AuthExample.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DTOs.Request;
using UrlShortener.Enums;
using UrlShortener.RepositoryContracts;
using UrlShortener.ServiceContracts;
using UrlShortener.Utilities;

namespace UrlShortener.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;

        public RegisterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegisterResult> RegisterUser(RegisterDTO registerDTO)
        {
            bool userExists = await _userRepository.IsUserExistsAsync(registerDTO.Username);
            if (userExists) // if user already exists 
            {
                return RegisterResult.Error;
            }
            string passwordHash = PasswordHasher.HashPassword(registerDTO.Password); // hash before storing in database
            var user = new User
            {
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }; // binding DTO to Model

            int id = await _userRepository.InsertNewUserAsync(user); //calling userRepository

            return (id != -1) ? RegisterResult.Success : RegisterResult.Error; // if some error occurs in userRepository

        }

        
    }
}


