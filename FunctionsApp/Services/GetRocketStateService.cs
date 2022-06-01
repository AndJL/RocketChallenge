using FunctionsApp.Models;
using FunctionsApp.Repository;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace FunctionsApp.Services
{
    public class GetRocketStateService : IGetRocketStateService
    {
        private readonly IRocketStateRepository _rocketStateRepository;
        private readonly IRocketMessageRepository _rocketMessageRepository;
        public GetRocketStateService(IRocketStateRepository rocketStateRepository, IRocketMessageRepository rocketMessageRepository)
        {
            _rocketStateRepository = rocketStateRepository;
            _rocketMessageRepository = rocketMessageRepository;
        }

        public async Task<RocketStateExtended> GetRocketState(string rocketId, bool extended)
        {
            var rocketState = await _rocketStateRepository.GetRocketState(rocketId);

            var rocketMessages = await _rocketMessageRepository.GetRocketMessages(rocketId);

            var sortedRocketMessages = rocketMessages.OrderBy(x => x.Metadata.MessageNumber).ToList();

            if(rocketState.History.Count != rocketMessages.Count() && rocketState.MessageNumber != sortedRocketMessages.Last().Metadata.MessageNumber)
            {
                //Something went wrong when updating the rocketstate 

                //Update RocketState according to the rocketMessages that we currently have
            }

            if (extended)
            {
                return new RocketStateExtended
                {
                    RocketState = rocketState,
                    RocketMessages = sortedRocketMessages
                };
            }
            else
            {
                return new RocketStateExtended
                {
                    RocketState = rocketState
                };
            }
        }
    }
}