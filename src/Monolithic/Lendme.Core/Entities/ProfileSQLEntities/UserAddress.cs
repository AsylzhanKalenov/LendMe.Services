namespace Lendme.Core.Entities.ProfileSQLEntities;

// Адреса (отдельная таблица для нормализации)
public class UserAddress
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public AddressType Type { get; set; } // Home, Work, Other
    public string Label { get; set; } // "Дом", "Работа", "Дача"
    public bool IsDefault { get; set; }
    
    // Адресные данные
    public string Country { get; set; } = "Kazakhstan";
    public string City { get; set; }
    public string District { get; set; }
    public string Street { get; set; }
    public string Building { get; set; }
    public string Apartment { get; set; }
    public string PostalCode { get; set; }
    
    // Геолокация
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    // Дополнительно
    public string ContactPhone { get; set; }
    public string DeliveryInstructions { get; set; }
    
    // Навигация
    public UserProfile Profile { get; set; }
}

public enum AddressType { Home, Work, Other }