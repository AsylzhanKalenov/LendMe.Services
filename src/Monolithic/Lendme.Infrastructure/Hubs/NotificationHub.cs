using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Lendme.Infrastructure.Hubs;

public class NotificationHub : Hub
{
    // Статический словарь для отслеживания активных подключений
    // userId -> список connectionIds
    private static readonly ConcurrentDictionary<Guid, HashSet<string>> _userConnections = new();

    public override Task OnConnectedAsync()
    {
        // Получаем userId из Claims или query string
        var userId = GetUserId();
        
        if (userId != Guid.Empty)
        {
            _userConnections.AddOrUpdate(
                userId,
                new HashSet<string> { Context.ConnectionId },
                (key, existing) =>
                {
                    existing.Add(Context.ConnectionId);
                    return existing;
                });
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        
        if (userId != Guid.Empty)
        {
            if (_userConnections.TryGetValue(userId, out var connections))
            {
                connections.Remove(Context.ConnectionId);
                
                // Если у пользователя не осталось активных подключений, удаляем запись
                if (connections.Count == 0)
                {
                    _userConnections.TryRemove(userId, out _);
                }
            }
        }

        return base.OnDisconnectedAsync(exception);
    }

    // Метод для проверки, подключен ли пользователь
    public static bool IsUserConnected(Guid userId)
    {
        return _userConnections.TryGetValue(userId, out var connections) && connections.Any();
    }

    // Получение всех connectionIds пользователя
    public static IEnumerable<string> GetUserConnectionIds(Guid userId)
    {
        return _userConnections.TryGetValue(userId, out var connections) 
            ? connections.ToList() 
            : Enumerable.Empty<string>();
    }

    private Guid GetUserId()
    {
        var userIdClaim = Context.User?.FindFirst("userId")?.Value 
                         ?? Context.User?.FindFirst("sub")?.Value;
        
        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }
        
        return Guid.Empty;
    }
}