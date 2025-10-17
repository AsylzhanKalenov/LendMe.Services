using System.Text.Json;
using Confluent.Kafka;
using Lendme.Application.Notification.Interface;
using Microsoft.Extensions.Configuration;

namespace Lendme.Infrastructure.Services.KafkaService;

public class KafkaProducerService : IKafkaProducerService, IDisposable
{
    private readonly IProducer<string, string> _producer;

    public KafkaProducerService(IConfiguration configuration)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
            ClientId = "lendme-producer",
            Acks = Acks.All,
            EnableIdempotence = true,
            CompressionType = CompressionType.Snappy
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default)
    {
        var messageJson = JsonSerializer.Serialize(message);
        
        var kafkaMessage = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = messageJson,
            Timestamp = Timestamp.Default
        };

        await _producer.ProduceAsync(topic, kafkaMessage, cancellationToken);
    }

    public void Dispose()
    {
        _producer?.Flush(TimeSpan.FromSeconds(10));
        _producer?.Dispose();
    }
}