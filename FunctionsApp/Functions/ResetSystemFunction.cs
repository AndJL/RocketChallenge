using FunctionsApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FunctionsApp.Functions
{
    public class ResetSystemFunction
    {
        private readonly IMongoDatabase _database;
        public ResetSystemFunction(IMongoDatabase database)
        {
            _database = database;
        }

        [FunctionName(nameof(ResetSystem))]
        public async Task ResetSystem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reset")] HttpRequest req
            )
        {
            //Reset system
            await _database.GetCollection<RocketMessage>("RocketMessages").DeleteManyAsync(Builders<RocketMessage>.Filter.Empty);
            await _database.GetCollection<RocketState>("RocketStates").DeleteManyAsync(Builders<RocketState>.Filter.Empty);

        }
    }
}