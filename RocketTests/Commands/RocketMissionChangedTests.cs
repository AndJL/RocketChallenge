using FakeItEasy;
using FunctionsApp.Commands;
using FunctionsApp.Models;
using FunctionsApp.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RocketTests.Commands
{
    public class RocketMissionChangedTests
    {
        [Fact]
        public void RocketMissionChanged_CallsRepositoryUpdate()
        {
            //Arrange
            var rocketMessage = new RocketMessage
            {
                Metadata = new Metadata
                {
                    Channel = Guid.NewGuid().ToString(),
                    MessageNumber = 50,
                    MessageTime = DateTime.Now
                },
                Message = new Message
                {
                    NewMission = "GoToPluto"
                }
            };

            var rocketState = new RocketState
            {
                RocketId = rocketMessage.Metadata.Channel,
                MessageNumber = 49
            };

            var fakeRocketRepository = A.Fake<IRocketStateRepository>();

            A.CallTo(() => fakeRocketRepository.GetRocketState(rocketMessage.Metadata.Channel)).Returns(rocketState);

            var target = new RocketMissionChangedCommand(rocketMessage);

            //Act
            var actual = target.Execute(fakeRocketRepository);

            //Assert
            A.CallTo(() => fakeRocketRepository.UpdateRocketState(A<UpdateDefinition<RocketState>>._, rocketState.RocketId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void RocketMissionChanged_RocketHasAlreadyRecievedThisMessage_DoesNotCallRepositoryUpdate()
        {
            //Arrange
            var rocketMessage = new RocketMessage
            {
                Metadata = new Metadata
                {
                    Channel = Guid.NewGuid().ToString(),
                    MessageNumber = 50,
                    MessageTime = DateTime.Now
                },
                Message = new Message
                {
                    NewMission = "GoToPluto"
                }
            };

            var rocketState = new RocketState
            {
                RocketId = rocketMessage.Metadata.Channel,
                MessageNumber = 50
            };

            var fakeRocketRepository = A.Fake<IRocketStateRepository>();

            A.CallTo(() => fakeRocketRepository.GetRocketState(rocketMessage.Metadata.Channel)).Returns(rocketState);

            var target = new RocketMissionChangedCommand(rocketMessage);

            //Act
            var actual = target.Execute(fakeRocketRepository);

            //Assert
            A.CallTo(() => fakeRocketRepository.UpdateRocketState(A<UpdateDefinition<RocketState>>._, rocketState.RocketId)).MustNotHaveHappened();
        }
    }
}