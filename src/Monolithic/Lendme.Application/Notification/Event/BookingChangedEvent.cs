using Lendme.Application.Notification.Interface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Lendme.Application.Notification.Event;

public class BookingChangedEvent : INotification
{
    public Guid BookingId { get; set; }
    public string BookingNumber { get; set; }
    public Guid RentId { get; set; }
    public Guid ItemId { get; set; }
    public Guid RenterId { get; set; }
    public Guid OwnerId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public class Handler : INotificationHandler<BookingChangedEvent>
    {
        private readonly IKafkaProducerService _kafkaProducer;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IKafkaProducerService kafkaProducer,
            ILogger<Handler> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task Handle(BookingChangedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _kafkaProducer.PublishAsync("booking-changed", notification, cancellationToken);
            
                _logger.LogInformation(
                    "Published BookingCreatedEvent for booking {BookingNumber} to Kafka",
                    notification.BookingNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish BookingCreatedEvent to Kafka");
                throw;
            }
        }
    }
}