using Lendme.Core.Entities.Booking;
using Lendme.Core.Interfaces.BookingRepositories;

namespace Lendme.Infrastructure.SqlPersistence.Repository;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Booking> AddBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        await _context.Bookings.AddAsync(booking, cancellationToken);
        return booking;
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        return await _context.Bookings.FindAsync(bookingId, cancellationToken);
    }

    public async Task<Booking> UpdateBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        var existingBooking = await _context.Bookings.FindAsync(booking.Id);
        if (existingBooking == null)
        {
            // TODO: Consider throwing a custom exception
            throw new Exception($"Booking with ID {booking.Id} not found");
        }

        _context.Entry(existingBooking).CurrentValues.SetValues(booking);
    
        return existingBooking;
    }

    public async Task DeleteBookingAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null)
        {
            throw new Exception($"Booking with ID {bookingId} not found");
        }

        if (booking.Status == BookingStatus.InProgress || booking.Status == BookingStatus.Confirmed)
        {
            throw new InvalidOperationException("Cannot delete active or confirmed booking. Cancel it first.");
        }

        booking.IsDeleted = true;
        booking.DeletedAt = DateTimeOffset.UtcNow;

    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}