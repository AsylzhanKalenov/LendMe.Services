namespace Lendme.Core.Entities;

public class Item
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int OwnerId { get; set; }
    public User User { get; set; }
}