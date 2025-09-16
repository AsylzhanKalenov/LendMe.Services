namespace Lendme.Core.Entities.Catalog;

public class Location
{
    public string Type { get; set; } = "Point";
    //public double[] Coordinates { get; set; } // [longitude, latitude]
    public double Longitude { get; private set; }
    public double Latitude { get; private set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; } // Delivery radius
    
    private Location() { }
    
    public Location(
        double longitude,
        double latitude,
        string address,
        string city,
        string district,
        int radiusMeters)
    {
        Longitude = longitude;
        Latitude = latitude;
        Address = address;
        City = city;
        District = district;
        RadiusMeters = radiusMeters;
    }
    
    public double[] GetCoordinates() => new[] { Longitude, Latitude };
}