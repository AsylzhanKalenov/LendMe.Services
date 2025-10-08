using Lendme.Application.Booking.Dto.Response;
using Lendme.Core.Entities.Booking;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using MediatR;

namespace Lendme.Application.Booking.Commands.Update;

public class OwnerReturnItemConfirmCommand : IRequest<OwnerReturnItemConfirmResponse>
{
    public Guid BookingId { get; set; }

    public class Handler : IRequestHandler<OwnerReturnItemConfirmCommand, OwnerReturnItemConfirmResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public Handler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        
        public async Task<OwnerReturnItemConfirmResponse> Handle(OwnerReturnItemConfirmCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, cancellationToken);
        
            if (booking.Status != BookingStatus.RETURN_PENDING)
                throw new InvalidOperationException("Можно подтвердить только ожидающее бронирование");
            
            booking.ChangeStatus(BookingStatus.COMPLETED);
            await _bookingRepository.UpdateBookingAsync(booking, cancellationToken);
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            
            // TODO: Notification to renter
            
            return new OwnerReturnItemConfirmResponse
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                ConfirmedAt = booking.ConfirmedAt
            };
        }
    }
}