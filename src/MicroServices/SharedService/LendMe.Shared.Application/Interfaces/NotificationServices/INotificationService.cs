using LendMe.Shared.Application.Notification.Dto;

namespace LendMe.Shared.Application.Interfaces.NotificationServices;

public interface INotificationService
{
    Task NotifyBookingChange(string deviceToken, BookingDto booking);
    Task NotifyUserBookingChange(Guid userId, string deviceToken, BookingDto booking);
}