namespace LendMe.Catalog.Application.Dto.Create;

public class CreateItemLocationDto
{
    public double[] Coordinates { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int RadiusMeters { get; set; }
}