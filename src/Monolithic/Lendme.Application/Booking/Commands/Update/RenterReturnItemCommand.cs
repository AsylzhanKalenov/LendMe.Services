using Lendme.Application.Booking.Dto.Response;
using Lendme.Core.Entities.Booking;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using MediatR;

namespace Lendme.Application.Booking.Commands.Update;

public class RenterReturnItemCommand : IRequest<RenterReturnItemResponse>
{
    public Guid BookingId { get; set; }

    public class Handler : IRequestHandler<RenterReturnItemCommand, RenterReturnItemResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public Handler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        
        public async Task<RenterReturnItemResponse> Handle(RenterReturnItemCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, cancellationToken);
        
            if (booking.Status != BookingStatus.IN_RENTAL)
                throw new InvalidOperationException("Можно подтвердить только ожидающее бронирование");
            
            booking.ChangeStatus(BookingStatus.RETURN_PENDING);
            await _bookingRepository.UpdateBookingAsync(booking, cancellationToken);
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            
            // TODO: Notification to renter
            
            return new RenterReturnItemResponse
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                ConfirmedAt = booking.ConfirmedAt
            };
        }
    }
}