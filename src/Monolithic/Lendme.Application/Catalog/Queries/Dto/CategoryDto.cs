namespace Lendme.Application.Catalog.Queries.Dto;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public Guid? ParentId { get; set; }
    public string IconUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    
    // Self-referencing
    public CategoryDto Parent { get; set; }
    public ICollection<CategoryDto> Children { get; set; }
}