using System.Text.Json;
using Lendme.Core.Entities.Booking;
using Lendme.Core.Entities.Profile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendme.Infrastructure.SqlPersistence.Configurations;

public class ItemHandoverConfiguration : IEntityTypeConfiguration<ItemHandover>
{
    public void Configure(EntityTypeBuilder<ItemHandover> builder)
    {
        builder.ToTable("item_handovers");

        builder.HasKey(h => h.Id);

        // Простые свойства
        builder.Property(h => h.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(h => h.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Вложенный объект Location
        builder.OwnsOne(h => h.Location, location =>
        {
            location.Property(l => l.Method).HasColumnName("location_method");
            
            // Convert Address to JSON
            location.Property(l => l.Address)
                .HasColumnName("location_address")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<Address>(v, (JsonSerializerOptions?)null)
                );
            
            location.Property(l => l.MeetingPoint).HasColumnName("location_meeting_point");
            location.Property(l => l.Instructions).HasColumnName("location_instructions");
            location.Property(l => l.Latitude).HasColumnName("location_latitude");
            location.Property(l => l.Longitude).HasColumnName("location_longitude");
            
            // Convert GeoLocation to JSON
            location.Property(l => l.Coordinates)
                .HasColumnName("location_coordinates")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<GeoLocation>(v, (JsonSerializerOptions?)null)
                );
        });

        // Вложенный объект Condition
        builder.OwnsOne(h => h.Condition, condition =>
        {
            condition.Property(c => c.Grade).HasColumnName("condition_grade");
            condition.Property(c => c.Description).HasColumnName("condition_description");
            condition.Property(c => c.HasAllAccessories).HasColumnName("condition_has_accessories");
            condition.Property(c => c.IsClean).HasColumnName("condition_is_clean");
            condition.Property(c => c.IsFunctional).HasColumnName("condition_is_functional");

            // Списки как JSON
            condition.Property(c => c.MissingAccessories)
                .HasColumnName("condition_missing_accessories")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null)
                );
                
            // Convert Damages list to JSON
            condition.Property(c => c.Damages)
                .HasColumnName("condition_damages")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<DamageReport>>(v, (JsonSerializerOptions?)null)
                );
        });

        // Вложенный объект Verification
        builder.OwnsOne(h => h.Verification, verification =>
        {
            verification.Property(v => v.IsOwnerPresent).HasColumnName("verify_owner_present");
            verification.Property(v => v.IsRenterPresent).HasColumnName("verify_renter_present");
            verification.Property(v => v.OwnerSignature).HasColumnName("verify_owner_signature");
            verification.Property(v => v.RenterSignature).HasColumnName("verify_renter_signature");
            verification.Property(v => v.SignedAt).HasColumnName("verify_signed_at");
            verification.Property(v => v.VerificationCode).HasColumnName("verify_code");
            verification.Property(v => v.IsDisputed).HasColumnName("verify_is_disputed");
            verification.Property(v => v.DisputeReason).HasColumnName("verify_dispute_reason");
        });

        // Convert Photos list to JSON
        builder.Property(h => h.Photos)
            .HasColumnName("photos")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<HandoverPhoto>>(v, (JsonSerializerOptions?)null)
            );

        // Связь с Booking
        builder.HasOne<Booking>()
            .WithMany()
            .HasForeignKey(h => h.BookingId);

        // Индексы
        builder.HasIndex(h => h.BookingId);
        builder.HasIndex(h => new { h.Type, h.Status });
    }
}