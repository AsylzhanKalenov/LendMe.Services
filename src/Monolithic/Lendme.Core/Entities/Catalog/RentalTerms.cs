namespace Lendme.Core.Entities.Catalog;

public class RentalTerms
{
    public string PickupInstructions { get; set; }
    public string UsageGuidelines { get; set; }
    public List<string> IncludedAccessories { get; set; }
    public string CancellationPolicy { get; set; }
    public bool RequiresDeposit { get; set; }
    public bool RequiresInsurance { get; set; }
    public List<string> RestrictedUses { get; set; }
    
    public RentalTerms(
        string pickupInstructions,
        string usageGuidelines,
        List<string> includedAccessories,
        string cancellationPolicy,
        bool requiresDeposit,
        bool requiresInsurance,
        List<string> restrictedUses)
    {
        PickupInstructions = pickupInstructions;
        UsageGuidelines = usageGuidelines;
        IncludedAccessories = includedAccessories ?? new List<string>();
        CancellationPolicy = cancellationPolicy;
        RequiresDeposit = requiresDeposit;
        RequiresInsurance = requiresInsurance;
        RestrictedUses = restrictedUses ?? new List<string>();
    }
}