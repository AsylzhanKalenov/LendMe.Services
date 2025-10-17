using Lendme.Core.Entities.Booking;

namespace Lendme.Core.Interfaces.Services.NotificationServices;

public interface INotificationService
{
    Task NotifyBookingChange(string deviceToken, Booking booking);
    Task NotifyUserBookingChange(Guid userId, string deviceToken, Booking booking);
}