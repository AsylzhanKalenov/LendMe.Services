namespace LendMe.Catalog.Application.Dto.Create;

public class CreateItemRentalTermsDto
{
    public string PickupInstructions { get; set; }
    public string UsageGuidelines { get; set; }
    public List<string> IncludedAccessories { get; set; }
    public string CancellationPolicy { get; set; }
    public bool RequiresDeposit { get; set; }
    public bool RequiresInsurance { get; set; }
    public List<string> RestrictedUses { get; set; }
}