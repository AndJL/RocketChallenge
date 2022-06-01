using FunctionsApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionsApp.Repository
{
    public class RocketMessageRepository : IRocketMessageRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<RocketMessage> _collection;
            
        public RocketMessageRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<RocketMessage>("RocketMessages");
        }

        public async Task<IEnumerable<RocketMessage>> GetRocketMessages(string rocketId)
        {
            return await _collection.Find(x => x.Metadata.Channel == rocketId).ToListAsync();
        }
    }
}
