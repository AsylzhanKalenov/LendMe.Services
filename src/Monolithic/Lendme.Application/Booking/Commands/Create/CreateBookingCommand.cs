using Lendme.Application.Booking.Dto.Response;
using Lendme.Core.Interfaces.Repositories.BookingRepositories;
using MediatR;

namespace Lendme.Application.Booking.Commands.Create;

public class CreateBookingCommand : IRequest<CreateBookingResponse>
{
    public Guid ItemId { get; set; }
    public Guid RenterId { get; set; }
    public Guid OwnerId { get; set; }
    
    public class Hanlder : IRequestHandler<CreateBookingCommand, CreateBookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public Hanlder(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        
        public async Task<CreateBookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.AddBookingAsync(new Core.Entities.Booking.Booking()
            {
                // TODO: Consider booking number format and generation
                BookingNumber = Guid.NewGuid().ToString(),
                ItemId = request.ItemId,
                RenterId = request.RenterId,
                OwnerId = request.OwnerId,
                CreatedAt = DateTime.UtcNow,
                Status = Core.Entities.Booking.BookingStatus.Pending
            }, cancellationToken);

            
            // TODO: Notification to owner

            return new CreateBookingResponse
            {
                Id = booking.Id,
                BookingNumber = booking.BookingNumber,
                CreatedAt = booking.CreatedAt
            };
        }
    }
}