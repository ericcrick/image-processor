namespace ImageProsessing.API.Services.Interfaces;

public interface IQueuesManagement
{
    public Task<bool> SendMessage<T>(T serviceMessage, string queueName, string connectionString);  
}
