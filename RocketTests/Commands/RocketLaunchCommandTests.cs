using FakeItEasy;
using FunctionsApp.Commands;
using FunctionsApp.Models;
using FunctionsApp.Repository;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RocketTests.Commands
{
    public class RocketLaunchCommandTests
    {
        [Fact]
        public void RocketLaunched_RocketHasNotBeenLaunchedYet_CallsRepositoryInsert()
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

            var target = new RocketLaunchedCommand(rocketMessage);

            //Act
            var actual = target.Execute(fakeRocketRepository);

            //Assert
            A.CallTo(() => fakeRocketRepository.InsertRocketState(A<RocketState>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void RocketLaunched_RocketHasBeenLaunchedAlready_DoesNotCallRepository()
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
                    By = 5000
                }
            };

            var fakeRocketRepository = A.Fake<IRocketStateRepository>();

            var target = new RocketLaunchedCommand(rocketMessage);

            //Act
            var actual = target.Execute(fakeRocketRepository);

            //Assert
            A.CallTo(() => fakeRocketRepository.InsertRocketState(A<RocketState>._)).MustNotHaveHappened();
        }
    }
}
