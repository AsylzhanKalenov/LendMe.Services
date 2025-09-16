namespace Lendme.Application.Booking.Dto.Request;

public class CreateBookingRequest
{
    public Guid ItemId { get; set; }
    public Guid RenterId { get; set; }
    public Guid OwnerId { get; set; }
}