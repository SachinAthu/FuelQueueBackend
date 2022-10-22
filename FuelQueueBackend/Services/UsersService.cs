using FuelQueueBackend.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FuelQueueBackend.Services
{
    public class UsersService
    {
        private IMongoCollection<User> _usersCollection = null!;

        public UsersService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            try
            {
                // MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
                // IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
                // _usersCollection = database.GetCollection<User>(mongoDBSettings.Value.UsersCollectionName);

                _usersCollection = new MongoDBService(mongoDBSettings).getInstance<User>(mongoDBSettings.Value.UsersCollectionName);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

        }

        // get all users
        public async Task<List<User>> GetAllUsers()
        {
            Console.WriteLine("called");

            try
            {
                return await _usersCollection.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // get user by id
        public async Task<User?> GetUserById(string id)
        {
            try
            {
                FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
                long count = await _usersCollection.Find(filter).CountDocumentsAsync();
                
                // not user found
                if(count == 0) return null;
                
                return await _usersCollection.Find(filter).FirstAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // create/register user
        public async Task<int> CreateUser(User user)
        {
            try
            {
                FilterDefinition<User> filter = Builders<User>.Filter.Eq("email", user.Email);
                long count = await _usersCollection.Find(filter).CountDocumentsAsync();

                // user already exists
                if (count > 0) return 1;

                await _usersCollection.InsertOneAsync(user);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // update user
        public async Task<int> UpdateUser(string id, User user)
        {
            try
            {
                FilterDefinition<User> filter1 = Builders<User>.Filter.Eq("Id", id);
                long count1 = await _usersCollection.Find(filter1).CountDocumentsAsync();

                // no user found
                if(count1 == 0) return 1;

                FilterDefinition<User> filter = Builders<User>.Filter.Eq("email", user.Email);
                long count2 = await _usersCollection.Find(filter).CountDocumentsAsync();

                // user already exists
                if (count2 > 0) return 2;

                await _usersCollection.ReplaceOneAsync(filter1, user);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // delete user
        public async Task<int> DeleteUser(string id)
        {
            try
            {
                FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
                long count = await _usersCollection.Find(filter).CountDocumentsAsync();

                // no user found
                if (count == 0) return 1;

                await _usersCollection.DeleteOneAsync(filter);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // login
        public async Task<string?> LoginUser(User user)
        {
            try
            {
                // check credentials
                if (user.Email == null || user.Password == null || user.Type == null) return "1";

                FilterDefinition<User> emailFilter = Builders<User>.Filter.Eq("email", user.Email);
                FilterDefinition<User> passwordFilter = Builders<User>.Filter.Eq("password", user.Password);
                FilterDefinition<User> typeFilter = Builders<User>.Filter.Eq("type", user.Type);
                FilterDefinition<User> filter = Builders<User>.Filter.And(emailFilter, passwordFilter, typeFilter);
                long count = await _usersCollection.Find(filter).CountDocumentsAsync();

                // invalid credentials, no user found
                if (count == 0) return "2";

                // create JWT token
                var builder = WebApplication.CreateBuilder();

                var issuer = builder.Configuration["Jwt:Issuer"];
                var audience = builder.Configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return jwtToken;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
