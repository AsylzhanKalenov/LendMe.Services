namespace Lendme.Core.Entities.Profile;

public class UserStatistics
{
    // Статистика владельца
    public OwnerStatistics AsOwner { get; set; }
    
    // Статистика арендатора
    public RenterStatistics AsRenter { get; set; }
    
    // Общая статистика
    public GeneralStatistics General { get; set; }
}

public class OwnerStatistics
{
    public int TotalListings { get; set; }
    public int ActiveListings { get; set; }
    public int CompletedRentals { get; set; }
    public int CancelledRentals { get; set; }
    public decimal TotalEarnings { get; set; }
    public decimal ThisMonthEarnings { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public TimeSpan AverageResponseTime { get; set; }
    public double ResponseRate { get; set; } // Процент ответов на запросы
    public double AcceptanceRate { get; set; } // Процент принятых бронирований
}

public class RenterStatistics
{
    public int CompletedRentals { get; set; }
    public int CancelledRentals { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal ThisMonthSpent { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public int FavoriteItems { get; set; }
    public Dictionary<string, int> RentalsByCategory { get; set; } // Категория -> Количество
}

public class GeneralStatistics
{
    public DateTime MemberSince { get; set; }
    public int ProfileViews { get; set; }
    public int ReportsMade { get; set; }
    public int ReportsReceived { get; set; }
    public List<UserBadge> EarnedBadges { get; set; }
    public int TrustScore { get; set; } // 0-100
}

public class UserBadge
{
    public string Code { get; set; } // SUPER_HOST, TRUSTED_RENTER, QUICK_RESPONDER
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public BadgeLevel Level { get; set; } // Bronze, Silver, Gold
    public DateTime EarnedAt { get; set; }
    public Dictionary<string, object> Criteria { get; set; } // Критерии получения
}
public enum BadgeLevel { Bronze, Silver, Gold, Platinum }