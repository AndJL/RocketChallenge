using FunctionsApp.Models;
using FunctionsApp.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionsApp.Functions
{
    public class UpdateStateFunction
    {
        private readonly IUpdateRocketStateService _updateRocketStateService;
        public UpdateStateFunction(IUpdateRocketStateService updateRocketStateService)
        {
            _updateRocketStateService = updateRocketStateService;
        }

        [FunctionName(nameof(UpdateRocketStates))]
        public async Task UpdateRocketStates(
            [ServiceBusTrigger("%event-queue%", Connection = "ServiceBusConnectionString")] RocketMessage queueItem,
            ILogger log)
        {
            log.LogInformation($"Running Rocket message with event ID: {queueItem._id}");

            await _updateRocketStateService.UpdateRocketState(queueItem);
        }
    }
}
