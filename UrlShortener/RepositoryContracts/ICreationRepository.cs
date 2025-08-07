using AuthExample.Models;
using UrlShortener.DTOs.Response;

namespace UrlShortener.RepositoryContracts
{
    public interface ICreationRepository
    {
        public Task<int> InsertNewCreation(Creation creation) ;

        

        public void DeleteCreation(int CreationId) ;

        public Task<List<Creation>> GetAllCreationsByUserId(int UserId) ;
        public Task<Creation> GetOriginalUrlFromShortId(string ShortId);
    }
}
