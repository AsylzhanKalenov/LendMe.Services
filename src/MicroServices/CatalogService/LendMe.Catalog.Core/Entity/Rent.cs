namespace LendMe.Catalog.Core.Entity;

public class Rent
{
    public Guid Id { get; set; }
    public Guid? CategoryId { get; set; }
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
    public DateTimeOffset UpdatedAt { get; set; }
    public NetTopologySuite.Geometries.Point Points { get; set; }
    public Category Category { get; set; }
    public RentalTerms Terms { get; set; }
    
    public void UpdateBasicInfo(string? title, string? description)
    {
        if (title is not null)
        {
            var t = title.Trim();
            if (t.Length == 0)
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            Title = t;
        }

        if (description is not null)
        {
            var d = description.Trim();
            // Описание может быть пустым, если требуется очистить
            Description = d;
        }
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateCategory(Guid? categoryId)
    {
        CategoryId = categoryId;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdatePriceRange(double? minPrice, double? maxPrice)
    {
        var newMin = minPrice ?? MinPrice;
        var newMax = maxPrice ?? MaxPrice;

        if (newMin < 0 || newMax < 0)
            throw new ArgumentOutOfRangeException("Price cannot be negative.");

        if (newMin > newMax)
            throw new ArgumentException("MinPrice cannot be greater than MaxPrice.");

        MinPrice = newMin;
        MaxPrice = newMax;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateLocation(
        double? longitude,
        double? latitude,
        string? address,
        string? city,
        string? district,
        int? radiusMeters)
    {
        var newLon = longitude ?? Longitude;
        var newLat = latitude ?? Latitude;

        if (longitude is not null && (newLon < -180 || newLon > 180))
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be in range [-180, 180].");

        if (latitude is not null && (newLat < -90 || newLat > 90))
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be in range [-90, 90].");

        if (radiusMeters is not null && radiusMeters < 0)
            throw new ArgumentOutOfRangeException(nameof(radiusMeters), "RadiusMeters cannot be negative.");

        Longitude = newLon;
        Latitude = newLat;

        if (address is not null) Address = address.Trim();
        if (city is not null) City = city.Trim();
        if (district is not null) District = district.Trim();
        if (radiusMeters is not null) RadiusMeters = radiusMeters.Value;

        Points = new NetTopologySuite.Geometries.Point(newLon, newLat) { SRID = 4326 };
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ReplaceTerms(RentalTerms? terms)
    {
        if (terms is null)
        {
            Terms = null!;
            return;
        }
        Terms = terms;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}