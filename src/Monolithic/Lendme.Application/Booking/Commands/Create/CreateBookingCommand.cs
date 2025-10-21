using Lendme.Application.Booking.Dto.Response;
using Lendme.Application.Notification.Event;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using MediatR;

namespace Lendme.Application.Booking.Commands.Create;

public class CreateBookingCommand : IRequest<CreateBookingResponse>
{
    public Guid RentId { get; set; }
    public Guid ItemId { get; set; }
    public Guid RenterId { get; set; }
    public Guid OwnerId { get; set; }
    
    public class Hanlder : IRequestHandler<CreateBookingCommand, CreateBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPublisher _publisher;

        public Hanlder(IBookingRepository bookingRepository, IPublisher publisher)
        {
            _bookingRepository = bookingRepository;
            _publisher = publisher;
        }
        
        public async Task<CreateBookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.AddBookingAsync(new Core.Entities.Booking.Booking()
            {
                // TODO: Consider booking number format and generation
                BookingNumber = Guid.NewGuid().ToString(),
                RentId = request.RentId,
                ItemId = request.ItemId,
                RenterId = request.RenterId,
                OwnerId = request.OwnerId,
                CreatedAt = DateTime.UtcNow,
                Status = Core.Entities.Booking.BookingStatus.HOLD_PENDING
            }, cancellationToken);

            await _bookingRepository.SaveChangesAsync(cancellationToken);

            return new CreateBookingResponse
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}