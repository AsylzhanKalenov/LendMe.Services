namespace Lendme.Core.Entities.Profile;

public class Address
{
    public string Country { get; set; } = "Kazakhstan";
    public string City { get; set; }
    public string District { get; set; }
    public string Street { get; set; }
    public string Building { get; set; }
    public string Apartment { get; set; }
    public string PostalCode { get; set; }
    public GeoLocation Coordinates { get; set; }
}