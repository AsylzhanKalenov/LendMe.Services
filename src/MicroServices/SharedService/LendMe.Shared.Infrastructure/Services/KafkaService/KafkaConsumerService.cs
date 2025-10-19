using System.Text.Json;
using Confluent.Kafka;
using LendMe.Shared.Application.Interfaces.NotificationServices;
using LendMe.Shared.Application.Notification.Dto;
using LendMe.Shared.Application.Notification.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LendMe.Shared.Infrastructure.Services.KafkaService;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<KafkaConsumerService> logger)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092",
            GroupId = "lendme-notification-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            EnableAutoOffsetStore = false
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("booking-created");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                if (consumeResult?.Message?.Value != null)
                {
                    await HandleMessageAsync(consumeResult.Message.Value, stoppingToken);

                    _consumer.Commit(consumeResult);
                    _consumer.StoreOffset(consumeResult);
                }
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Error consuming message from Kafka");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
            }
        }
    }

    private async Task HandleMessageAsync(string messageValue, CancellationToken cancellationToken)
    {
        var bookingCreatedEvent = JsonSerializer.Deserialize<BookingChangedEvent>(messageValue);

        if (bookingCreatedEvent == null)
        {
            _logger.LogWarning("Failed to deserialize BookingCreatedEvent");
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        // Создаем минимальный объект Booking для уведомления
        var booking = new BookingDto()
        {
            Id = bookingCreatedEvent.BookingId,
            BookingNumber = bookingCreatedEvent.BookingNumber,
            OwnerId = bookingCreatedEvent.OwnerId,
            RenterId = bookingCreatedEvent.RenterId,
            ItemId = bookingCreatedEvent.ItemId,
            Status = BookingStatus.HOLD_PENDING
        };

        // Отправляем уведомление владельцу
        // TODO: Получить deviceToken владельца из базы данных
        string ownerDeviceToken = ""; // Получите токен из репозитория пользователей

        await notificationService.NotifyUserBookingChange(
            bookingCreatedEvent.OwnerId,
            ownerDeviceToken,
            booking
        );

        _logger.LogInformation(
            "Notification sent to owner {OwnerId} for booking {BookingNumber}",
            bookingCreatedEvent.OwnerId,
            bookingCreatedEvent.BookingNumber
        );
    }

    public override void Dispose()
    {
        _consumer?.Close();
        _consumer?.Dispose();
        base.Dispose();
    }
}