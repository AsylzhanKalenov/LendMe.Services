namespace LendMe.Catalog.Application.Queries.Dto;

public class RentDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "Point";
    public double Longitude { get; private set; }
    public double Latitude { get; private set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; } // Delivery radius
    public NetTopologySuite.Geometries.Location Points { get; set; }
    public RentalTermsDto Terms { get; set; }
}