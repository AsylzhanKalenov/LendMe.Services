namespace Lendme.Application.Booking.Dto.Response;

public class ConfirmRenterResponse
{
    public Guid Id { get; set; }
    public string BookingNumber { get; set; }
    public DateTimeOffset? ConfirmedAt { get; set; }
}