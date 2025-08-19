namespace Lendme.Core.Interfaces.Booking;

public interface IBookingRepository
{
    Task<Entities.Booking.Booking> AddBookingAsync(Entities.Booking.Booking booking);
    Task<Entities.Booking.Booking?> GetBookingByIdAsync(Guid bookingId);
}