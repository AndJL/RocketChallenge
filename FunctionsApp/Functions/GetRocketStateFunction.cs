using FunctionsApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionsApp.Functions
{
    public class GetRocketStateFunction
    {
        private readonly IGetRocketStateService _getRocketStateService;
        public GetRocketStateFunction(IGetRocketStateService getRocketStateService)
        {
            _getRocketStateService = getRocketStateService;
        }

        [FunctionName(nameof(GetRocketState))]
        public async Task<IActionResult> GetRocketState(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rocketState/{rocketId}/{extended}")] HttpRequest req,
            ILogger log,
            string rocketId,
            bool extended = false)
        {
            log.LogInformation($"Returning current rocketState for rocket: {rocketId}");

            var rocketState = await _getRocketStateService.GetRocketState(rocketId, extended);

            return new OkObjectResult(rocketState);
        }
    }
}