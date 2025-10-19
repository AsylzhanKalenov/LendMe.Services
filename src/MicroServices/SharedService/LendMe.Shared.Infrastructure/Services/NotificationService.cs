using FirebaseAdmin.Messaging;
using LendMe.Shared.Application.Interfaces.NotificationServices;
using LendMe.Shared.Application.Notification.Dto;
using LendMe.Shared.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace LendMe.Shared.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _signalR;
    private readonly FirebaseMessaging _fcm;

    public async Task NotifyBookingChange(string deviceToken, BookingDto booking)
    {
        // Определяем получателей уведомления (владелец и арендатор)
        var recipientIds = new[] { booking.OwnerId, booking.RenterId };

        foreach (var userId in recipientIds)
        {
            // Проверяем, подключен ли пользователь к SignalR
            if (NotificationHub.IsUserConnected(userId))
            {
                // Пользователь онлайн - отправляем только через SignalR
                var connectionIds = NotificationHub.GetUserConnectionIds(userId);
                await _signalR.Clients.Clients(connectionIds.ToList())
                    .SendAsync("BookingStatusChanged", booking.BookingNumber, booking.Status);
            }
            else
            {
                // Пользователь офлайн - отправляем push через FCM
                await SendProductStatusNotification(deviceToken, booking.BookingNumber, booking.Status);
            }
        }
    }

    public async Task NotifyUserBookingChange(Guid userId, string deviceToken, BookingDto booking)
    {
        if (NotificationHub.IsUserConnected(userId))
        {
            // Отправляем только через SignalR
            var connectionIds = NotificationHub.GetUserConnectionIds(userId);
            await _signalR.Clients.Clients(connectionIds.ToList())
                .SendAsync("BookingStatusChanged", booking.BookingNumber, booking.Status);
        }
        else
        {
            // Отправляем только через FCM
            await SendProductStatusNotification(deviceToken, booking.BookingNumber, booking.Status);
        }
    }
    
    private async Task SendProductStatusNotification(string deviceToken, string bookingNumber, BookingStatus newStatus)
    {
        var message = new Message()
        {
            Token = deviceToken,
            Notification = new Notification
            {
                Title = "Статус брони изменен",
                Body = $"Бронь #{bookingNumber} теперь: {GetStatusDisplayName(newStatus)}"
            },
            Data = new Dictionary<string, string>()
            {
                { "bookingNumber", bookingNumber },
                { "status", newStatus.ToString() },
                { "type", "booking_status_change" }
            }
        };

        await FirebaseMessaging.DefaultInstance.SendAsync(message);
    }
    private async Task SendFCMNotification(List<string> deviceTokens, string productId, string newStatus)
    {
        // Разбить на батчи по 500 токенов (ограничение FCM)
        var batches = deviceTokens.Chunk(500);
        
        foreach (var batch in batches)
        {
            var message = new MulticastMessage()
            {
                Tokens = deviceTokens,
                Notification = new Notification
                {
                    Title = "Статус товара изменен",
                    Body = $"Товар #{productId} теперь: {newStatus}"
                },
                Data = new Dictionary<string, string>()
                {
                    { "productId", productId },
                    { "status", newStatus },
                    { "type", "status_change" }
                }
            };
            
            var response = await FirebaseMessaging.DefaultInstance
                .SendMulticastAsync(message);
            
            // Удалить недействительные токены
            if (response.FailureCount > 0)
            {
                await RemoveInvalidTokens(response, batch.ToList());
            }
        }
    }
    
    private async Task RemoveInvalidTokens(BatchResponse response, List<string> tokens)
    {
        for (int i = 0; i < response.Responses.Count; i++)
        {
            if (!response.Responses[i].IsSuccess)
            {
                var errorCode = response.Responses[i].Exception?.MessagingErrorCode;
                
                // Удалить если токен устарел или приложение удалено
                if (errorCode == MessagingErrorCode.InvalidArgument ||
                    errorCode == MessagingErrorCode.Unregistered)
                {
                    var tokenToRemove = tokens[i];
                    // TODO: Delete device tokens from db
                    // var dbToken = await _context.DeviceTokens
                    //     .FirstOrDefaultAsync(t => t.Token == tokenToRemove);
                    //
                    // if (dbToken != null)
                    // {
                    //     _context.DeviceTokens.Remove(dbToken);
                    // }
                }
            }
        }
    }
    
    private string GetStatusDisplayName(BookingStatus status)
    {
        return status switch
        {
            BookingStatus.AWAIT_OWNER_CONFIRMATION => "Ожидает подтверждения владельца",
            BookingStatus.CONFIRMED_READY => "Подтверждена",
            BookingStatus.IN_RENTAL => "В аренде",
            BookingStatus.COMPLETED => "Завершена",
            BookingStatus.CANCELLED_BY_RENTER => "Отменена арендатором",
            BookingStatus.CANCELLED_BY_OWNER => "Отменена владельцем",
            _ => status.ToString()
        };
    }
}