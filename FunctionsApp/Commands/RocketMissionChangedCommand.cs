using FunctionsApp.Models;
using FunctionsApp.Repository;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionsApp.Commands
{
    public class RocketMissionChangedCommand : RocketCommand
    {
        private readonly RocketMessage _rocketMessage;
        public RocketMissionChangedCommand(RocketMessage rocketMessage)
        {
            _rocketMessage = rocketMessage;
        }

        public override async Task Execute(IRocketStateRepository rocketStateRepository)
        {
            var rocketState = await rocketStateRepository.GetRocketState(_rocketMessage.Metadata.Channel);

            if (rocketState != null && ShouldUpdateRocketState(rocketState.MessageNumber, _rocketMessage.Metadata.MessageNumber))
            {
                var update = Builders<RocketState>.Update
                            .Set(x => x.Mission, _rocketMessage.Message.NewMission)
                            .Set(x => x.LastTransmissionMsg, $"Mission has changed! oldMission: {rocketState.Mission}, newMission: {_rocketMessage.Message.NewMission}")
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
