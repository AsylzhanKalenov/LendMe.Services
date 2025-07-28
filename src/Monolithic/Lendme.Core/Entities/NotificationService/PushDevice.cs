namespace Lendme.Core.Entities.NotificationService;

public class PushDevice
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string DeviceToken { get; set; }
    public DevicePlatform Platform { get; set; }
    public string AppVersion { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime LastActiveAt { get; set; }
    public bool IsActive { get; set; }
}