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
    public class RocketExplodedCommandTests
    {
        [Fact]
        public void RocketExploded_CallsRepositoryUpdate()
        {
            //Arrange
            var rocketMessage = new RocketMessage
            {
                Metadata = new Metadata
                {
                    Channel = Guid.NewGuid().ToString(),
                    MessageNumber = 99,
                    MessageTime = DateTime.Now
                },
                Message = new Message
                {
                    Reason = "ENGINE FAILURE!"
                }
            };

            var rocketState = new RocketState
            {
                RocketId = rocketMessage.Metadata.Channel,
                MessageNumber = 10
            };

            var fakeRocketRepository = A.Fake<IRocketStateRepository>();

            A.CallTo(() => fakeRocketRepository.GetRocketState(rocketMessage.Metadata.Channel)).Returns(rocketState);

            var target = new RocketExplodedCommand(rocketMessage);

            //Act
            var actual = target.Execute(fakeRocketRepository);

            //Assert
            A.CallTo(() => fakeRocketRepository.UpdateRocketState(A<UpdateDefinition<RocketState>>._, rocketState.RocketId)).MustHaveHappenedOnceExactly();
        }
    }
}