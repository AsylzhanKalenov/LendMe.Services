namespace Lendme.Core.Entities.Profile;

public class LocationInfo
{
    public Address PrimaryAddress { get; set; }
    public GeoLocation DefaultLocation { get; set; }
}