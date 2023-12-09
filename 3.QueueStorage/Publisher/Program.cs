using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string connectionString = "<YOUR_CONNECTION_STRING_OR_SAS>";
string queueName = "test-queue";
string message = "this is a test message!";
QueueClient queueClient = new QueueClient(connectionString, queueName);

queueClient.CreateIfNotExists();
// Send a message to the queue
queueClient.SendMessage(message);

// Get Queue properties
QueueProperties properties = queueClient.GetProperties();

// Retrieve the cached approximate message count.
int cachedMessagesCount = properties.ApproximateMessagesCount; 
Console.WriteLine($"Number of messages in queue: {cachedMessagesCount}");


// Peek at the next message
//You can peek at the messages in the queue without removing them from the queue by calling the PeekMessages method
PeekedMessage[] peekedMessage = queueClient.PeekMessages();

// Get the message from the queue
QueueMessage[] messageQueue = queueClient.ReceiveMessages();

// Update the message contents
queueClient.UpdateMessage(messageQueue[0].MessageId,
        messageQueue[0].PopReceipt,
        "Updated contents",
        TimeSpan.FromSeconds(60.0)  // Make it invisible for another 60 seconds
    );


// Get the message from the queue
messageQueue = queueClient.ReceiveMessages();

// Process (i.e. print) the message in less than 30 seconds
Console.WriteLine($"Dequeued message: '{messageQueue[0].Body}'");

// Delete the message
queueClient.DeleteMessage(messageQueue[0].MessageId, messageQueue[0].PopReceipt);

Console.WriteLine("Press any key to delete the Queue");

// Delete the queue
queueClient.Delete();