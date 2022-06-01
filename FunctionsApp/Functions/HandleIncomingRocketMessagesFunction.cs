using FunctionsApp.Models;
using FunctionsApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace FunctionsApp.Functions
{
    public class HandleIncomingRocketMessagesFunction
    {
        private readonly IRocketMessageService _rocketMessageService;
        public HandleIncomingRocketMessagesFunction(IRocketMessageService rocketMessage)
        {
            _rocketMessageService = rocketMessage;
        }

        [FunctionName(nameof(RecieveMessages))] 
        public async Task<IActionResult> RecieveMessages(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "messages")] HttpRequest req,
            [ServiceBus("%event-queue%", Connection = "ServiceBusConnectionString")] IAsyncCollector<RocketMessage> eventQueue,
            ILogger log)
        {
            var messageAsJson = await req.ReadAsStringAsync();

            log.LogInformation($"Recieving message from Rocket: {messageAsJson}");

            var model = JsonConvert.DeserializeObject<RocketMessage>(messageAsJson);
            try
            {
                log.LogInformation($"Handling message: {model.Metadata.Channel}");

                await _rocketMessageService.SaveRocketMessageToEventStore(model);

                await eventQueue.AddAsync(model);

                return new OkResult();
            }
            catch (Exception e)
            {
                //Handle exception by logging message to an error coll.
                log.LogError($"An error has occurred, pushing message to error coll");
                
                //Handle exception case is out of scope for this challenge

                return new OkResult();
            }
        }
    }
}
