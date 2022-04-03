using ImageProsessing.API.Services.Interfaces;
using Azure.Storage.Queues;
using System.Text.Json;
namespace ImageProsessing.API.Services;

public class QueuesManagement: IQueuesManagement
{
    public async Task<bool> SendMessage<T>(T serviceMessage, string queueName, string connectionString){
        // create a queue client
        var queueClient = new QueueClient(connectionString, queueName);
        // serialize message Body since it's generic
        var messageBody = JsonSerializer.Serialize(serviceMessage);
        await queueClient.SendMessageAsync(messageBody);
        return true;
    }
}