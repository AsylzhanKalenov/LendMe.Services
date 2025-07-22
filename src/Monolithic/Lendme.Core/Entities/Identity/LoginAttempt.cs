namespace Lendme.Core.Entities.Identity;

public class LoginAttempt
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public bool IsSuccessful { get; set; }
    public DateTime AttemptedAt { get; set; }
    public string FailureReason { get; set; }
}