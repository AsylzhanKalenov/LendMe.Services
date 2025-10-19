namespace LendMe.Shared.Application.Interfaces.NotificationServices;

public interface IKafkaProducerService
{
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default);
}