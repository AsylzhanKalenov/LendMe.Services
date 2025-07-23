namespace Lendme.Core.Entities.Profile;

// MongoDB
public class UserProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } // Связь с Identity Service
    
    // Базовая информация
    public PersonalInfo PersonalInfo { get; set; }
    public UserType Type { get; set; } // Renter, Owner, Both
    
    // Локация и адреса
    public LocationInfo LocationInfo { get; set; }
    
    // Верификация
    public VerificationInfo Verification { get; set; }
    
    // Настройки
    public UserPreferences Preferences { get; set; }
    
    // Статистика (денормализованная)
    public UserStatistics Statistics { get; set; }
    
    // Метаданные
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? LastActiveAt { get; set; }
}

public enum UserType { Renter, Owner, Both }