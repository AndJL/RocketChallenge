using FunctionsApp.Models;
using FunctionsApp.Repository;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionsApp.Commands
{
    public class RocketLaunchedCommand : RocketCommand
    {
        private readonly RocketMessage _rocketMessage;
        public RocketLaunchedCommand(RocketMessage rocketMessage)
        {
            _rocketMessage = rocketMessage;
        }

        public override async Task Execute(IRocketStateRepository rocketStateRepository)
        {
            if (!HasRocketBeenLaunchedAlready(_rocketMessage.Metadata.MessageNumber))
            {
                var rocketState = new RocketState
                {
                    RocketId = _rocketMessage.Metadata.Channel,
                    LastTransmissionMsg = "Houston, we have lift off!",
                    MessageNumber = 1,
                    Mission = _rocketMessage.Message.Mission,
                    Speed = _rocketMessage.Message.LaunchSpeed,
                    Type = _rocketMessage.Message.Type,
                    Updated = _rocketMessage.Metadata.MessageTime,
                    History = new List<RocketMessage> { _rocketMessage }
                };

                await rocketStateRepository.InsertRocketState(rocketState);
            }
            else
            {
                //Rocket has already been launched and should not be launched again.
            }
        }

        private bool HasRocketBeenLaunchedAlready(int messageNumber)
        {
            return messageNumber > 1;
        }
    }
}
