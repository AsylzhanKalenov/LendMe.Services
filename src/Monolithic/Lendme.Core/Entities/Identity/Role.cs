namespace Lendme.Core.Entities.Identity;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } // Admin, Moderator, User
    public ICollection<Permission> Permissions { get; set; }
}