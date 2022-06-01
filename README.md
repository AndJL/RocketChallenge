# Rocket Challenge App built on Azure Functions with MongoDb transactions

## Endpoints
### baseUrl
If run locally baseurl should be: http://localhost:7071
On Azure: https://func-rocketchallenge.azurewebsites.net/

- /api/rockets: 
   - Gives a list of all Rockets.
- /api/rocketState/{rocketId}/{extended}: 
   - Gets a rockets current state(its eventually consistent). Use true/false as "extended" parameter to get the recieved rocketMessages for debugging purposes.
- /api/messages:
  - Endpoint to recieve incoming messages from rockets.

- /api/reset:
   - To reset the system (Delete all rocket messages and rocketStates) Cannot be undone!

## How it works
1. After recieve a rocket message the azure function inserts the event into the eventstore and puts an event into the azure servicebus queue.
2. Another azure function then reacts to messages in the servicebus queue and determines which command to fire onwards.
3. The servicebustriggered azure function then sends an insert/update to the mongoDB server, but it could be sending this command to another azure function or some other application if need be. For this assignment I've decided to just fire at the mongoDB using transactions.

### Hosting
Azure functions is being hosted on a free azure subscription
Azure Servicebus Queue is being hosted on a free azure subscription
MongoDB is being hosted on cloud.mongodb.com on a free mongo atlas subscription
Therefore services might be down if the subscriptions run out of credits.

Reason to use Azure as the host is to use the Azure auto scaling functions that you get from using a cloud platform. Azure functions (if setup to do so) auto scale and just put another instance into the mix to get more firepower.

## Comments 
I've decided that exception handling is currently out of scope for this assignment, but should in a producation enviroment be implemented.

Also in the 'GetRocketStateService' class i've decided not to go further into implementing an update case for the state if the state has run out of sync.

First time programmering something using mongoDB transactions which has been alittle tricky, but I think it worked out in the end ;)
