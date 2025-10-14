using Microsoft.AspNetCore.SignalR;

namespace Lendme.Infrastructure.Hubs;

public class NotificationHub : Hub
{
    public async Task SendProductUpdate(string productId, string status)
    {
        await Clients.All.SendAsync("ReceiveProductUpdate", productId, status);
    }
}