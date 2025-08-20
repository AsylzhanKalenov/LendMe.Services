namespace Lendme.Application.Booking.Dto.Response;

public class ConfirmBookingResponse
{
    public Guid Id { get; set; }
    public string BookingNumber { get; set; }
    public DateTimeOffset? ConfirmedAt { get; set; }
}