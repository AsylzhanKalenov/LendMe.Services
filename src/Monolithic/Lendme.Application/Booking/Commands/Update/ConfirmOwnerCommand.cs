using Lendme.Application.Booking.Dto.Response;
using Lendme.Application.Notification.Event;
using Lendme.Core.Entities.Booking;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using MediatR;

namespace Lendme.Application.Booking.Commands.Update;

public class ConfirmOwnerCommand : IRequest<ConfirmOwnerResponse>
{
    public Guid BookingId { get; set; }

    public class Handler : IRequestHandler<ConfirmOwnerCommand, ConfirmOwnerResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPublisher _publisher;

        public Handler(IBookingRepository bookingRepository, IPublisher publisher)
        {
            _bookingRepository = bookingRepository;
            _publisher = publisher;
        }

        public async Task<ConfirmOwnerResponse> Handle(ConfirmOwnerCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, cancellationToken);

            if (booking.Status != BookingStatus.HOLD_PENDING)
                throw new InvalidOperationException("Можно подтвердить только ожидающее бронирование");

            booking.ChangeStatus(BookingStatus.CONFIRMED_READY);
            await _bookingRepository.UpdateBookingAsync(booking, cancellationToken);
            await _bookingRepository.SaveChangesAsync(cancellationToken);

            // TODO: Notification to renter
            var bookingCreatedEvent = new BookingChangedEvent
            {
                BookingId = booking.Id,
                BookingNumber = booking.BookingNumber,
                RentId = booking.RentId,
                ItemId = booking.ItemId,
                RenterId = booking.RenterId,
                OwnerId = booking.OwnerId,
                CreatedAt = booking.CreatedAt
            };

            await _publisher.Publish(bookingCreatedEvent, cancellationToken);

            return new ConfirmOwnerResponse
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                ConfirmedAt = booking.ConfirmedAt
            };
        }
    }
}