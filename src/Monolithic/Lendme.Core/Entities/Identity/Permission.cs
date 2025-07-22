namespace Lendme.Core.Entities.Identity;

public class Permission
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Resource { get; set; }
    public string Action { get; set; }
}