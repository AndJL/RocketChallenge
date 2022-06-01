using FunctionsApp.Repository;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace FunctionsApp.Commands
{
    public abstract class RocketCommand
    {
        public abstract Task Execute(IRocketStateRepository rocketStateRepository);

        protected bool ShouldUpdateRocketState(int rocketStateMessageNumber, int rocketMessageMessageNumber)
        {
            return rocketMessageMessageNumber > rocketStateMessageNumber;
        }
    }
}
