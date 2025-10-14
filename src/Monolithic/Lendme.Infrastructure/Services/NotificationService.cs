using FirebaseAdmin.Messaging;
using Lendme.Core.Entities.Booking;
using Lendme.Core.Interfaces.Services.NotificationServices;
using Lendme.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Lendme.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _signalR;
    private readonly FirebaseMessaging _fcm;

    public async Task NotifyBookingChange(string deviceToken, Booking booking)
    {
        // 1. Отправить через SignalR (мгновенно для активных пользователей)
        await _signalR.Clients.All.SendAsync("ProductChanged", booking.BookingNumber, booking.Status);

        // 2. Отправить push (для неактивных пользователей)
        await SendProductStatusNotification(deviceToken, booking.BookingNumber, booking.Status);
    }
    
    private async Task SendProductStatusNotification(string deviceToken, string productId, BookingStatus newStatus)
    {
        var message = new Message()
        {
            Token = deviceToken,
            Notification = new Notification
            {
                Title = "Статус товара изменен",
                Body = $"Товар #{productId} теперь: {newStatus}"
            },
            Data = new Dictionary<string, string>()
            {
                { "productId", productId },
                { "status", newStatus.ToString() },
                { "type", "status_change" }
            }
        };

        string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
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
}