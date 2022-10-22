using FuelQueueBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FuelQueueBackend.Services
{
    public class MongoDBService
    {
        IMongoDatabase database;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            try
            {
                MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
                database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public IMongoCollection<T> getInstance<T>(string value)
        {
            return database.GetCollection<T>(value);
        }
    }
}
