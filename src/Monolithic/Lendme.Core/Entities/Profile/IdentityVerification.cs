namespace Lendme.Core.Entities.Profile;

public class IdentityVerification
{
    public bool IsVerified { get; set; }
    public string IIN { get; set; } // Encrypted
    public DateTime? VerifiedAt { get; set; }
    public string VerificationMethod { get; set; } // "GovDatabase", "Manual", "AI"
}