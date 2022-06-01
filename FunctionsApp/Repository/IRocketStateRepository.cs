using FunctionsApp.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FunctionsApp.Repository
{
    public interface IRocketStateRepository 
    {
        Task InsertRocketState(RocketState rocketState);
        IClientSessionHandle GetSession();
        Task<RocketState> GetRocketState(string rocketId);
        Task UpdateRocketState(UpdateDefinition<RocketState> updateDefinition, string rocketId);

        Task<IEnumerable<RocketState>> GetAllRocketStates();
    }
}