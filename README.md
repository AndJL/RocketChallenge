# Rocket Challenge App built on Azure Functions with MongoDb transactions, written in C#

## Endpoints
### baseUrl
If run locally baseurl should be: http://localhost:7071
On Azure: https://func-rocketchallenge.azurewebsites.net/

- GET /api/rockets: 
   - Gives a list of all Rockets.
- GET /api/rocketState/{rocketId}/{extended}: 
   - Gets a rockets current state(its eventually consistent). Use true/false as "extended" parameter to get the recieved rocketMessages for debugging purposes.
- POST /api/messages:
  - Endpoint to recieve incoming messages from rockets.

- POST /api/reset:
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

## Consideration
The very simplified way to determine if a rocketstate should be updated, needs to be made better for a real world application.

Consider having a list of 3 messages incoming. As the assignment suggest, we cant be sure that the messages are coming in the correct order.
Having a list of 3 messages (For simplication we work with numbers, see below):

Message List: [ 2 , 1 , 3 ]

Looking at the implementation, we would assume that the first message (2) would not be updated to the rocketstate because there isn't a rocketstate to start with.
Also the messagenumber: 2, is greater than 1 and would return a false in method 'HasRocketBeenLaunchedAlready' in 'RocketLaunchedCommand.cs'

We would go on to the second message (1), and seeing that this is in fact the first datapoint from the rocket, and it would create the rocketstate.

The last message would be read, and the rocketstate would be updated with the third message (3).

So we end up having message two (1) and three (3), updated to the rocketstate and message 1 (2) gone from the state.
RocketState ends up being inconsistent which is not what we want :/ 


