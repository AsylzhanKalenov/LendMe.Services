namespace Lendme.Core.Entities.Identity;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public string DeviceId { get; set; }
    public string DeviceInfo { get; set; }
    public bool IsRevoked { get; set; }
}