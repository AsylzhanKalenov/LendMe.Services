using Lendme.Core.Entities.Booking;

namespace Lendme.Core.Interfaces.Repositories.BookingRepositories;

public interface IBookingRepository
{
    Task<Booking> AddBookingAsync(Booking booking, CancellationToken cancellationToken);
    Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken);
    Task<Booking> UpdateBookingAsync(Booking booking, CancellationToken cancellationToken);
    Task DeleteBookingAsync(Guid bookingId, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}