using FunctionsApp.Models;
using FunctionsApp.Commands;
using FunctionsApp.Models.Enums;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FunctionsApp.Repository;

namespace FunctionsApp.Services
{
    public class UpdateRocketStateService : IUpdateRocketStateService
    {
        private readonly IRocketStateRepository _rocketStateRepository;
        public UpdateRocketStateService(IRocketStateRepository rocketStateRepository)
        {
            _rocketStateRepository = rocketStateRepository;
        }

        public async Task UpdateRocketState(RocketMessage rocketMessage)
        {
            await GetRocketCommand(rocketMessage).Execute(_rocketStateRepository);
        }

        public RocketCommand GetRocketCommand(RocketMessage rocketMessage)
        {
            switch (rocketMessage.Metadata.MessageType)
            {
                case MessageType.RocketLaunched:
                    return new RocketLaunchedCommand(rocketMessage);

                case MessageType.RocketSpeedIncreased:
                    return new UpdateRocketVelocityCommand(rocketMessage, VelocityType.Increased);

                case MessageType.RocketSpeedDecreased:
                    return new UpdateRocketVelocityCommand(rocketMessage, VelocityType.Decreased);

                case MessageType.RocketExploded:
                    return new RocketExplodedCommand(rocketMessage);

                case MessageType.RocketMissionChanged:
                    return new RocketMissionChangedCommand(rocketMessage);

                default:
                    return null;
            }
        }
    }
}
