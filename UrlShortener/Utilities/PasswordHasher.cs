using Microsoft.AspNetCore.Identity;
namespace UrlShortener.Utilities
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            string hashedPassword = hasher.HashPassword(null, password);
            return hashedPassword;
        }

        public static bool VerifyHashedPassword(string hashedPassword, string inputPassword)
        {
            var hasher = new PasswordHasher<object>();
            var result = hasher.VerifyHashedPassword(null, hashedPassword, inputPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
