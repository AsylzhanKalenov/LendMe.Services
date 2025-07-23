namespace Lendme.Core.Entities.Profile;

public class UserPreferences
{
    // Общие настройки
    public GeneralPreferences General { get; set; }
    
    // Настройки уведомлений
    public NotificationPreferences Notifications { get; set; }
    
    // Настройки приватности
    public PrivacyPreferences Privacy { get; set; }
    
    // Настройки для владельцев
    public OwnerPreferences OwnerSettings { get; set; }
    
    // Настройки для арендаторов
    public RenterPreferences RenterSettings { get; set; }
}

public class GeneralPreferences
{
    public string Language { get; set; } = "ru";
    public string Currency { get; set; } = "KZT";
    public string TimeZone { get; set; } = "Asia/Almaty";
    public string DateFormat { get; set; } = "dd.MM.yyyy";
}

public class NotificationPreferences
{
    public bool EmailEnabled { get; set; } = true;
    public bool SmsEnabled { get; set; } = true;
    public bool PushEnabled { get; set; } = true;
    
    // Детальные настройки по типам
    public Dictionary<string, NotificationChannelPreference> ByType { get; set; }
}

public class NotificationChannelPreference
{
    public bool Email { get; set; }
    public bool Sms { get; set; }
    public bool Push { get; set; }
    public bool InApp { get; set; }
}

public class PrivacyPreferences
{
    public bool ShowPhoneNumber { get; set; } = false;
    public bool ShowFullName { get; set; } = false;
    public bool ShowAddress { get; set; } = false;
    public ProfileVisibility ProfileVisibility { get; set; } = ProfileVisibility.Public;
}

public class OwnerPreferences
{
    public bool AllowInstantBooking { get; set; } = false;
    public int MinRentalDays { get; set; } = 1;
    public int MaxRentalDays { get; set; } = 30;
    public TimeSpan ResponseTimeCommitment { get; set; } = TimeSpan.FromHours(1);
    public List<Guid> BlockedUsers { get; set; } = new();
}

public class RenterPreferences
{
    public int DefaultRentalDuration { get; set; } = 1;
    public List<Guid> FavoriteCategories { get; set; } = new();
    public int SearchRadius { get; set; } = 10; // км
    public bool SaveSearchHistory { get; set; } = true;
}

public enum ProfileVisibility { Public, RegisteredUsers, Private }