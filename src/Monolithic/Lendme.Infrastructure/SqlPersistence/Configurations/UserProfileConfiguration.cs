using System.Text.Json;
using Lendme.Core.Entities.ProfileSQLEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendme.Infrastructure.SqlPersistence.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        // JSONB колонки
        builder.Property(e => e.Preferences)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<UserPreferences>(v, (JsonSerializerOptions?)null)!
            );
            
        builder.Property(e => e.Metadata)
            .HasColumnType("jsonb");
            
        // Игнорируем списки ID
        builder.Ignore(e => e.Addresses);
        builder.Ignore(e => e.Documents);
    }
}