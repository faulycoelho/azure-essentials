//Create your resource using Azure CLI:
//myLocation =EastUs
//myNameSpaceName = az204svcbus$RANDOM

//az group create --name az204-svcbus-rg --location $myLocation

//az servicebus namespace create \
//    --resource-group az204-svcbus-rg \
//    --name $myNameSpaceName \
//    --location $myLocation

//az servicebus queue create --resource-group az204-svcbus-rg \
//    --namespace-name $myNameSpaceName \
//    --name az204-queue
using Azure.Messaging.ServiceBus;

string connectionString = "Shared access policies > RootManageSharedAccessKey > Primary Connection String";

// name of your Service Bus topic
string queueName = "az204-queue";

// the client that owns the connection and can be used to create senders and receivers
ServiceBusClient client;

// the sender used to publish messages to the queue
ServiceBusSender sender;

// Create the clients that we'll use for sending and processing messages.
client = new ServiceBusClient(connectionString);
sender = client.CreateSender(queueName);

// create a batch 
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <= 3; i++)
{
    // try adding a message to the batch
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
    {
        // if an exception occurs
        throw new Exception($"Exception {i} has occurred.");
    }
}

try
{
    // Use the producer client to send the batch of messages to the Service Bus queue
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of three messages has been published to the queue.");
}
finally
{
    // Calling DisposeAsync on client types is required to ensure that network
    // resources and other unmanaged objects are properly cleaned up.
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

Console.WriteLine("Follow the directions in the exercise to review the results in the Azure portal.");
Console.WriteLine("Press any key to continue");
Console.ReadKey();