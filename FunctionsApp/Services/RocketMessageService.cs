using FunctionsApp.Models;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FunctionsApp.Services
{
    public class RocketMessageService : IRocketMessageService
    {
        private readonly IMongoDatabase _database;

        public RocketMessageService(IMongoDatabase mongoDatabase)
        {
            _database = mongoDatabase;
        }

        public async Task SaveRocketMessageToEventStore(RocketMessage rocketMessage)
        {
            await GetMongoCollection<RocketMessage>("RocketMessages").InsertOneAsync(rocketMessage);
        }

        private IMongoCollection<T> GetMongoCollection<T>(string collectionName)
        {
            var collection = _database.GetCollection<T>(collectionName);
            return collection;
        }
    }
}