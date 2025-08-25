using AuthExample.Models;
using UrlShortener.DTOs.Response;

namespace UrlShortener.RepositoryContracts
{
    public interface ICreationRepository
    {
        public Task<int> InsertNewCreation(Creation creation); 
        public Task<Creation> GetOriginalUrlFromShortId(string ShortId);
        public Task<List<Creation>> GetUrlsByUserId(int UserId);
        public Task<bool> IsUrlOwnedByUser(int userId, int urlId);
    }
}
