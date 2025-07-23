namespace Lendme.Core.Entities.Profile;

public class PersonalInfo
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }
    public string CoverPhotoUrl { get; set; }
}

public enum Gender
{
    male, female
}