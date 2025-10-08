using Lendme.Application.Booking.Dto.Response;
using Lendme.Core.Entities.Booking;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Lendme.Application.Booking.Commands.Update;

public class UploadReceiptCommand : IRequest<UploadReceiptResponse>
{
    public Guid BookingId { get; set; }
    
    public IFormFile Check { get; set; }

    public class Handler : IRequestHandler<UploadReceiptCommand, UploadReceiptResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public Handler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        
        public async Task<UploadReceiptResponse> Handle(UploadReceiptCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetBookingByIdAsync(request.BookingId, cancellationToken);
        
            if (booking.Status != BookingStatus.HOLD_PENDING)
                throw new InvalidOperationException("Можно подтвердить только ожидающее бронирование");
            
            booking.ChangeStatus(BookingStatus.RECEIPT_UPLOADED);
            await _bookingRepository.UpdateBookingAsync(booking, cancellationToken);
            await _bookingRepository.SaveChangesAsync(cancellationToken);
            
            // TODO: Notification to renter
            return new UploadReceiptResponse
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                ConfirmedAt = booking.ConfirmedAt
            };
        }
    }
}