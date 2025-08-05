namespace Lendme.Application.Catalog.Queries.Dto;


// MongoDB
public class ItemDetailsDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; } // Link to PostgreSQL
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    
    public LocationDto Location { get; set; }
    public List<ItemImageDto> Images { get; set; }
    public RentalTermsDto Terms { get; set; }
}