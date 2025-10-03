namespace LendMe.Catalog.Core.Dto;

public class RentSearchResult
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; } // Delivery radius
    public DateTimeOffset CreatedAt { get; set; }
}