using FunctionsApp.Models;
using FunctionsApp.Models.Enums;
using FunctionsApp.Repository;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionsApp.Commands
{
    public class UpdateRocketVelocityCommand : RocketCommand
    {
        private readonly RocketMessage _rocketMessage;

        public readonly VelocityType _velocityType;

        public UpdateRocketVelocityCommand(RocketMessage rocketMessage, VelocityType velocityType)
        {
            _rocketMessage = rocketMessage;
            _velocityType = velocityType;
        }

        public override async Task Execute(IRocketStateRepository rocketStateRepository)
        {
            var rocketState = await rocketStateRepository.GetRocketState(_rocketMessage.Metadata.Channel);

            if (rocketState != null && ShouldUpdateRocketState(rocketState.MessageNumber, _rocketMessage.Metadata.MessageNumber))
            {
                int velocity = 0;

                if (_velocityType == VelocityType.Increased)
                {
                    velocity = rocketState.Speed + _rocketMessage.Message.By;
                    rocketState.LastTransmissionMsg = $"Rocket speed increased by: {_rocketMessage.Message.By}";
                }
                else
                {
                    velocity = rocketState.Speed - _rocketMessage.Message.By;
                    rocketState.LastTransmissionMsg = $"Rocket speed decreased by: {_rocketMessage.Message.By}";
                }

                UpdateDefinition<RocketState> update = Builders<RocketState>.Update
                    .Set(x => x.Speed, velocity)
                    .Set(x => x.LastTransmissionMsg, rocketState.LastTransmissionMsg)
                    .Set(x => x.Updated, _rocketMessage.Metadata.MessageTime)
                    .Set(x => x.MessageNumber, _rocketMessage.Metadata.MessageNumber)
                    .AddToSet(x => x.History, _rocketMessage);

                await rocketStateRepository.UpdateRocketState(update, _rocketMessage.Metadata.Channel);
            }
            else
            {
                //RocketState has already been updated with this rocketmessage... 
            }
        }
    }
}