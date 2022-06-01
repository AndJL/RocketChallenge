using FunctionsApp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionsApp.Repository
{
    public class RocketStateRepository : IRocketStateRepository
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<RocketState> _collection;
        private readonly IClientSessionHandle _clientSession;
        public RocketStateRepository(IMongoDatabase database)
        {
            _database = database;
            _collection = database.GetCollection<RocketState>("RocketStates");
            _clientSession = _database.Client.StartSession();
        }

        public async Task<IEnumerable<RocketState>> GetAllRocketStates()
        {
            var rocketStates = new List<RocketState>();
            await _clientSession.WithTransaction(async (s, ct) =>
            {
                var filter = Builders<RocketState>.Filter.Exists(x => x._id);
                rocketStates = await _collection.Find(filter).ToListAsync();
            },
            new TransactionOptions(),
            CancellationToken.None);

            return rocketStates;
        }

        public async Task<RocketState> GetRocketState(string rocketId)
        {
            var rocketState = new RocketState();
            await _clientSession.WithTransaction(async (s, ct) =>
            {
                rocketState = await _collection.Find(s, x => x.RocketId == rocketId).FirstOrDefaultAsync();
            },
            new TransactionOptions(),
            CancellationToken.None
            );

            return rocketState;
        }

        public IClientSessionHandle GetSession()
        {
            return _clientSession;
        }

        public async Task InsertRocketState(RocketState rocketState)
        {
            await _clientSession.WithTransaction(async (s, ct) =>
            {
                await _collection.InsertOneAsync(rocketState);
            },
            new TransactionOptions(),
            CancellationToken.None
            );
        }

        public async Task UpdateRocketState(UpdateDefinition<RocketState> updateDefinition, string rocketId)
        {
            await _clientSession.WithTransaction(async (s, ct) =>
            {
                var filter = Builders<RocketState>.Filter.Eq(x => x.RocketId, rocketId);
                await _collection.UpdateOneAsync(filter, updateDefinition);
            },
            new TransactionOptions(),
            CancellationToken.None
            );
        }
    }
}
