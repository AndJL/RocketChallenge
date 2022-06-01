using FunctionsApp.Models;
using FunctionsApp.Repository;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionsApp.Services
{
    public class GetRocketListService : IGetRocketListService
    {
        private readonly IRocketStateRepository _rocketStateRepository;
        public GetRocketListService(IRocketStateRepository rocketStateRepository)
        {
            _rocketStateRepository = rocketStateRepository;
        }

        public async Task<List<RocketState>> GetRocketList()
        {
            var rocketStates = await _rocketStateRepository.GetAllRocketStates();

            return rocketStates.OrderBy(x => x.Type).ToList();
        }
    }
}