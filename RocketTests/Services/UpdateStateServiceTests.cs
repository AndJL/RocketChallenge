using FakeItEasy;
using FunctionsApp.Commands;
using FunctionsApp.Models;
using FunctionsApp.Repository;
using FunctionsApp.Services;
using Xunit;

namespace RocketTests.Services
{
    public class UpdateStateServiceTests
    {
        [Fact]
        public void GetRocketCommand_MessageTypeIsLaunched_ReturnsRocketLaunchedCommand()
        {
            //Arrange
            var rocketMessage = new RocketMessage
            {
                Metadata = new Metadata
                {
                    Channel = Guid.NewGuid().ToString(),
                    MessageNumber = 1,
                    MessageType = MessageType.RocketLaunched,
                    MessageTime = DateTime.Now
                },
                Message = new Message
                {
                    LaunchSpeed = 500,
                    Mission = "ARTEMIS",
                    Type = "Falcon-9"
                }
            };

            var fakeRocketRepository = A.Fake<IRocketStateRepository>();

            var target = new UpdateRocketStateService(fakeRocketRepository);

            //Act
            var actual = target.GetRocketCommand(rocketMessage);

            //Assert
            Assert.Equal(typeof(RocketLaunchedCommand), actual.GetType());
        }

        [Fact]
        public void GetRocketCommand_MessageTypeIsExploded_ReturnsRocketExplodedCommand()
        {
            //Arrange
            var rocketMessage = new RocketMessage
            {
                Metadata = new Metadata
                {
                    Channel = Guid.NewGuid().ToString(),
                    MessageNumber = 1,
                    MessageType = MessageType.RocketExploded,
                    MessageTime = DateTime.Now
                },
                Message = new Message
                {
                    Reason = "LOST COMMUNICATION => EXPLODE!"
                }
            };

            var fakeRocketRepository = A.Fake<IRocketStateRepository>();

            var target = new UpdateRocketStateService(fakeRocketRepository);

            //Act
            var actual = target.GetRocketCommand(rocketMessage);

            //Assert
            Assert.Equal(typeof(RocketExplodedCommand), actual.GetType());
        }
    }
}
