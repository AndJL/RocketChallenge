using FunctionsApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionsApp.Functions
{
    public class GetRocketsFunction
    {
        private readonly IGetRocketListService _getRocketListService;
        public GetRocketsFunction(IGetRocketListService getRocketListService)
        {
            _getRocketListService = getRocketListService;
        }

        [FunctionName(nameof(GetRocketList))]
        public async Task<IActionResult> GetRocketList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rockets")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Returning all rockets");

            var rockets = await _getRocketListService.GetRocketList();

            return new OkObjectResult(rockets);
        }
    }
}