namespace Lendme.Core.Entities.Profile;

public class VerificationInfo
{
    // Is it needed?
    //public VerificationLevel Level { get; set; } // Basic, Verified, Trusted
    public IdentityVerification Identity { get; set; }
    public List<VerificationDocument> Documents { get; set; }
    public DateTime? LastVerificationAt { get; set; }
}