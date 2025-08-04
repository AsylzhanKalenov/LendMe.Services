namespace Lendme.Core.Entities.ProfileSQLEntities;

public class UserPreferences
{
    // Primary Key для EF Core
    public Guid Id { get; set; }
    
    // Foreign Key к пользователю
    public Guid ProfileId { get; set; }
    
    // Общие настройки
    public GeneralPreferences General { get; set; } = new();
    
    // Настройки уведомлений
    public NotificationPreferences Notifications { get; set; } = new();
    
    // Настройки приватности
    public PrivacyPreferences Privacy { get; set; } = new();
    
    // Настройки для владельцев
    public OwnerPreferences OwnerSettings { get; set; } = new();
    
    // Настройки для арендаторов
    public RenterPreferences RenterSettings { get; set; } = new();
    
    // Audit fields
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation property (если есть User entity)
    public UserProfile Profile { get; set; }
}

public class GeneralPreferences
{
    public string Language { get; set; } = "ru";
    public string Currency { get; set; } = "KZT";
    public string TimeZone { get; set; } = "Asia/Almaty";
    public string DateFormat { get; set; } = "dd.MM.yyyy";
    //public Theme Theme { get; set; } = Theme.Light;
}

public class NotificationPreferences
{
    public bool EmailEnabled { get; set; } = true;
    public bool SmsEnabled { get; set; } = true;
    public bool PushEnabled { get; set; } = true;
}

public class PrivacyPreferences
{
    public bool ShowPhoneNumber { get; set; } = false;
    public bool ShowFullName { get; set; } = false;
    public bool ShowAddress { get; set; } = false;
    public bool ShowOnlineStatus { get; set; } = true;
    public ProfileVisibility ProfileVisibility { get; set; } = ProfileVisibility.Public;
}

public class OwnerPreferences
{
    public bool AllowInstantBooking { get; set; } = false;
    public int MinRentalDays { get; set; } = 1;
    public int MaxRentalDays { get; set; } = 30;
    public int AdvanceBookingDays { get; set; } = 90;
    public bool AutoAcceptVerifiedRenters { get; set; } = false;
    public List<Guid> BlockedUsers { get; set; } = new();
}

public class RenterPreferences
{
    public int DefaultRentalDuration { get; set; } = 1;
    public List<Guid> FavoriteCategories { get; set; } = new();
    public int SearchRadius { get; set; } = 10; // км
    public bool SaveSearchHistory { get; set; } = true;
    public List<string> SavedSearches { get; set; } = new();
}


public enum ProfileVisibility { Public, RegisteredUsers, Private }