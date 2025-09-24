namespace Lendme.Core.Entities.Catalog;


// MongoDB
public class ItemDetails
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; } // Link to PostgreSQL
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    
    public List<ItemImage> Images { get; set; }
    
    private ItemDetails() 
    { 
        Tags = new List<string>();
    }
    
    public ItemDetails(
        Guid itemId,
        string description,
        List<string> tags)
    {
        Id = Guid.NewGuid();
        ItemId = itemId;
        Description = description;
        Tags = tags ?? new List<string>();
    }
}