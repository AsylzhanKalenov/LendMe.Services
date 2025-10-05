namespace LendMe.Catalog.Application.Dto.Update;

public class UpdateRentDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; } // Delivery radius
    public UpdateRentalTermsDto Terms { get; set; }
}