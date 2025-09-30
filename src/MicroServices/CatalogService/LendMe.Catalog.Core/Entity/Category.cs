namespace LendMe.Catalog.Core.Entity;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? ParentId { get; set; }
    public string? IconUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    // Self-referencing
    public Category? Parent { get; set; }
    public ICollection<Category> Children { get; set; }
}