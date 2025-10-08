using Lendme.Application.Booking.Dto.Response;
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

        public Handler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        
        public async Task<ConfirmOwnerResponse> Handle(ConfirmOwnerCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, cancellationToken);
        
            if (booking.Status != BookingStatus.RECEIPT_UPLOADED)
                throw new InvalidOperationException("Можно подтвердить только ожидающее бронирование");
            
            booking.ChangeStatus(BookingStatus.CONFIRMED_READY);
            await _bookingRepository.UpdateBookingAsync(booking, cancellationToken);
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            
            // TODO: Notification to renter
            
            return new ConfirmOwnerResponse
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                ConfirmedAt = booking.ConfirmedAt
            };
        }
    }
}