namespace LendMe.Catalog.Core.Entity;

public class Rent
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "Point";
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; } // Delivery radius
    public DateTimeOffset CreatedAt { get; set; }
    public NetTopologySuite.Geometries.Point Points { get; set; }
    public RentalTerms Terms { get; set; }
}