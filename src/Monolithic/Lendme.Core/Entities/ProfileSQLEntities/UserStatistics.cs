namespace Lendme.Core.Entities.ProfileSQLEntities;

// Статистика (материализованное представление или отдельная таблица)
public class UserStatistics
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    
    // Статистика владельца
    public int TotalListings { get; set; }
    public int ActiveListings { get; set; }
    public int CompletedRentalsAsOwner { get; set; }
    public int CancelledRentalsAsOwner { get; set; }
    public decimal TotalEarnings { get; set; }
    public decimal ThisMonthEarnings { get; set; }
    public decimal ThisYearEarnings { get; set; }
    public double AverageRatingAsOwner { get; set; }
    public int ReviewCountAsOwner { get; set; }
    public TimeSpan AverageResponseTime { get; set; }
    public double ResponseRate { get; set; }
    public double AcceptanceRate { get; set; }
    
    // Статистика арендатора
    public int CompletedRentalsAsRenter { get; set; }
    public int CancelledRentalsAsRenter { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal ThisMonthSpent { get; set; }
    public double AverageRatingAsRenter { get; set; }
    public int ReviewCountAsRenter { get; set; }
    
    // Общая статистика
    public DateTime MemberSince { get; set; }
    public int ProfileViews { get; set; }
    public int TrustScore { get; set; } // 0-100
    
    // Обновление
    public DateTime LastCalculatedAt { get; set; }
    
    // Навигация
    public UserProfile Profile { get; set; }
}