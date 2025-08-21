using Lendme.Application.Booking.Dto.Response;
using Lendme.Core.Entities.Booking;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using MediatR;

namespace Lendme.Application.Booking.Commands.Update;

public class ConfirmBookingCommand : IRequest<ConfirmBookingResponse>
{
    public Guid BookingId { get; set; }

    public class Handler : IRequestHandler<ConfirmBookingCommand, ConfirmBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public Handler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        
        public async Task<ConfirmBookingResponse> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, cancellationToken);
        
            if (booking.Status != BookingStatus.Pending)
                throw new InvalidOperationException("Можно подтвердить только ожидающее бронирование");
            
            booking.Confirm();
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            
            // TODO: Notification to renter
            
            return new ConfirmBookingResponse
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                ConfirmedAt = booking.ConfirmedAt
            };
        }
    }
}