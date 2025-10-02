namespace LendMe.Catalog.Application.Dto;

public class LocationDto
{
    public string Type { get; set; } = "Point";
    public double[] Coordinates { get; set; } // [longitude, latitude]
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; } // Delivery radius
}