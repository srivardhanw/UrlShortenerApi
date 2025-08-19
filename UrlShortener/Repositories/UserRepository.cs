
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using UrlShortener.DTOs.Request;
using UrlShortener.RepositoryContracts;
using Dapper;
using AuthExample.Models;
using System.Threading.Tasks;
using UrlShortener.DTOs.Response;

namespace UrlShortener.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection) { 
            _dbConnection = dbConnection;
        }

        public async Task<bool> IsUserExistsAsync(string username) // to check if username already exists in db
        {
            var query = "SELECT 1 FROM [User] WHERE Username = @Username";
            var user = await _dbConnection.QueryFirstOrDefaultAsync<int?>(query, new {Username = username});
            return user.HasValue;
        }

        public async Task<int> InsertNewUserAsync(User user) // Adding new user to db, returns the id of the inserted user, returns -1 if some error occurs in database
        {
            var query = @"INSERT INTO [User] (Username, Email, PasswordHash, CreatedAt, UpdatedAt) 
                         VALUES (@Username, @Email, @PasswordHash, @CreatedAt, @UpdatedAt)
                         SELECT CAST(SCOPE_IDENTITY() as int);";
            try
            {
                int id = await _dbConnection.QuerySingleAsync<int>(query,
                    new { Username = user.Username, Email = user.Email, PasswordHash = user.PasswordHash, CreatedAt = user.CreatedAt, UpdatedAt = user.UpdatedAt });
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not insert the new User", ex);
            }

        }

        public async Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            var query = "SELECT * FROM [User] WHERE Username = @Input OR Email = @Input";
            var user = await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { Input = usernameOrEmail });
            return user;
        }


              
       
    }
}
