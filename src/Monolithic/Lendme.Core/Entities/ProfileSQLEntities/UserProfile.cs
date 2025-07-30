using System.ComponentModel.DataAnnotations.Schema;

namespace Lendme.Core.Entities.ProfileSQLEntities;

public class UserProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } // FK к Identity Service
    
    // Основная информация (обычные колонки)
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string Bio { get; set; }
    public string AvatarUrl { get; set; }
    public string CoverPhotoUrl { get; set; }
    public UserType Type { get; set; } // Renter, Owner, Both
    
    // Верификация
    public VerificationLevel VerificationLevel { get; set; }
    public bool IsIdentityVerified { get; set; }
    public DateTime? IdentityVerifiedAt { get; set; }
    
    // JSONB колонки для гибких данных
    [Column(TypeName = "jsonb")]
    public UserPreferences Preferences { get; set; }
    
    [Column(TypeName = "jsonb")]
    public Dictionary<string, object> Metadata { get; set; }
    
    // Временные метки
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastActiveAt { get; set; }
    
    // Навигация
    public ICollection<UserAddress> Addresses { get; set; }
    public ICollection<VerificationDocument> Documents { get; set; }
    // Нужен ли какой то значок?
    //public ICollection<UserBadge> Badges { get; set; }
    public UserStatistics Statistics { get; set; }
}

public enum UserType { Renter, Owner, Both }
public enum Gender { Male, Female, Other, PreferNotToSay }

public enum VerificationLevel { Basic, Verified, Trusted, Premium }