using FakeItEasy;
using FunctionsApp.Commands;
using FunctionsApp.Models;
using FunctionsApp.Models.Enums;
using FunctionsApp.Repository;
using MongoDB.Driver;
using Xunit;

namespace RocketTests.Commands
{
    public class UpdateRocketVelocityCommandTests
    {
        [Fact]
        public void UpdateRocketVelocity_RocketHasBeenLaunched_CallsRepositoryUpdate()
        {
            //Arrange
            var rocketMessage = new RocketMessage
            {
                Metadata = new Metadata
                {
                    Channel = Guid.NewGuid().ToString(),
                    MessageNumber = 2,
                    MessageType = MessageType.RocketSpeedIncreased,
                    MessageTime = DateTime.Now
                },
                Message = new Message
                {
                    By = 1000
                }
            };

            var rocketState = new RocketState
            {
                RocketId = rocketMessage.Metadata.Channel,
                MessageNumber = 1
            };

            var fakeRocketRepository = A.Fake<IRocketStateRepository>();

            A.CallTo(() => fakeRocketRepository.GetRocketState(rocketMessage.Metadata.Channel)).Returns(rocketState);

            var target = new UpdateRocketVelocityCommand(rocketMessage, VelocityType.Increased);

            //Act
            var actual = target.Execute(fakeRocketRepository);

            //Assert
            A.CallTo(() => fakeRocketRepository.UpdateRocketState(A<UpdateDefinition<RocketState>>._ , rocketState.RocketId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void UpdateRocketVelocity_RocketHasNotBeenLaunched_DoesNotCallRepositoryUpdate()
        {
            //Arrange
            var rocketMessage = new RocketMessage
            {
                Metadata = new Metadata
                {
                    Channel = Guid.NewGuid().ToString(),
                    MessageNumber = 1,
                    MessageType = MessageType.RocketSpeedIncreased,
                    MessageTime = DateTime.Now
                },
                Message = new Message
                {
                    By = 1000
                }
            };

            var rocketState = new RocketState
            {
                RocketId = rocketMessage.Metadata.Channel,
                MessageNumber = 2
            };

            var fakeRocketRepository = A.Fake<IRocketStateRepository>();

            A.CallTo(() => fakeRocketRepository.GetRocketState(rocketMessage.Metadata.Channel)).Returns(rocketState);

            var target = new UpdateRocketVelocityCommand(rocketMessage, VelocityType.Increased);

            //Act
            var actual = target.Execute(fakeRocketRepository);

            //Assert
            A.CallTo(() => fakeRocketRepository.UpdateRocketState(A<UpdateDefinition<RocketState>>._, rocketState.RocketId)).MustNotHaveHappened();
        }
    }
}