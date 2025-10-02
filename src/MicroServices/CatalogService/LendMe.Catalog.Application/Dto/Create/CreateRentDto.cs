namespace LendMe.Catalog.Application.Dto.Create;

public class CreateRentDto
{
    public string Type { get; set; } = "Point";
    public double MinPrice { get; set; }
    public double MaxPrice { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; }
    public CreateRentalTermsDto Terms { get; set; }
}