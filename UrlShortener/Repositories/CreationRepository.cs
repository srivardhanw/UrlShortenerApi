using AuthExample.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using UrlShortener.DTOs.Response;
using UrlShortener.RepositoryContracts;

namespace UrlShortener.Repositories
{
    public class CreationRepository : ICreationRepository
    {
        private readonly IDbConnection _dbConnection;
            
        public CreationRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<int> InsertNewCreation(Creation creation)
        {
            var query = @"INSERT INTO Creation (OriginalUrl,ShortId, CreatedAt, CreatedBy) 
                          VALUES (@OriginalUrl, @ShortId, @CreatedAt, @CreatedBy)
                          SELECT CAST(SCOPE_IDENTITY() AS INT);";
            
            int id = await _dbConnection.QuerySingleAsync<int>(query,
                    new { OriginalUrl = creation.OriginalUrl, ShortId = creation.ShortId, CreatedAt = creation.CreatedAt, CreatedBy = creation.CreatedBy });
            return id;
            
            
        }
        

      

        public async Task<Creation> GetOriginalUrlFromShortId(string ShortId)
        {
            var query = @"SELECT * FROM Creation
                          WHERE ShortId = @ShortId";

            var OriginalUrl = await _dbConnection.QueryFirstOrDefaultAsync<Creation>(query, new { ShortId = ShortId });
            if (OriginalUrl == null) throw new KeyNotFoundException($"Original Url with Short Id: \"{ShortId}\" Not Found");
            return OriginalUrl; 

        }

        public async Task<List<Creation>> GetUrlsByUserId(int CreatedBy)
        {
            var query = @"SELECT * FROM Creation
                          WHERE CreatedBy = @CreatedBy";

            List<Creation> creations = (await _dbConnection.QueryAsync<Creation>(query, new {CreatedBy =  CreatedBy})).ToList();
            return creations;
        }

        public async Task<bool> IsUrlOwnedByUser(int userId, int urlId)
        {
            var query = @"SELECT CreatedBy FROM Creation
                          WHERE Id = @Id";
            var createdBy = await _dbConnection.QueryFirstOrDefaultAsync<int>(query, new { Id = urlId });
            return (createdBy == userId)? true: false;
        }

       

    }
}
