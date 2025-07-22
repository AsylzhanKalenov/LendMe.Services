namespace Lendme.Core.Entities;


// MongoDB
public class ItemDetails
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; } // Link to PostgreSQL
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    
    public Location Location { get; set; }
    public List<ItemImage> Images { get; set; }
    public RentalTerms Terms { get; set; }
}