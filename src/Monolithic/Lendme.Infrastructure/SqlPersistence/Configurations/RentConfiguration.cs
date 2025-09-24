using System.Text.Json;
using Lendme.Core.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendme.Infrastructure.SqlPersistence.Configurations;

public class RentConfiguration : IEntityTypeConfiguration<Rent>
{
    public void Configure(EntityTypeBuilder<Rent> builder)
    {
        builder.ToTable("Rent");
        
        builder.HasKey(x => x.Id);
        
        // Базовые свойства
        builder.Property(x => x.Id)
            .ValueGeneratedNever(); // Генерируется в конструкторе
        
        // Owned type для Location (в одной таблице)
        // builder.OwnsOne(x => x.Location, location =>
        // {
        //     location.Property(l => l.Type)
        //         .HasColumnName("location_type")
        //         .HasMaxLength(50)
        //         .HasDefaultValue("Point");
        //         
        //     location.Property(l => l.Longitude)
        //         .HasColumnName("location_longitude")
        //         .HasPrecision(18, 6);
        //         
        //     location.Property(l => l.Latitude)
        //         .HasColumnName("location_latitude")
        //         .HasPrecision(18, 6);
        //         
        //     location.Property(l => l.Address)
        //         .HasColumnName("location_address")
        //         .HasMaxLength(500);
        //         
        //     location.Property(l => l.City)
        //         .HasColumnName("location_city")
        //         .HasMaxLength(100);
        //         
        //     location.Property(l => l.District)
        //         .HasColumnName("location_district")
        //         .HasMaxLength(100);
        //         
        //     location.Property(l => l.RadiusMeters)
        //         .HasColumnName("location_radius_meters");
        //     
        //     location.HasIndex(l => new { l.Latitude, l.Longitude })
        //         .HasDatabaseName("IX_ItemDetails_Location_Coordinates");
        // });
        
        // Owned type для RentalTerms (в одной таблице)
        builder.OwnsOne(x => x.Terms, terms =>
        {
            terms.Property(t => t.PickupInstructions)
                .HasColumnName("terms_pickup_instructions")
                .HasMaxLength(1000);
                
            terms.Property(t => t.UsageGuidelines)
                .HasColumnName("terms_usage_guidelines")
                .HasMaxLength(2000);
                
            terms.Property(t => t.IncludedAccessories)
                .HasColumnName("terms_included_accessories")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
                )
                .HasColumnType("jsonb");
                
            terms.Property(t => t.CancellationPolicy)
                .HasColumnName("terms_cancellation_policy")
                .HasMaxLength(1000);
                
            terms.Property(t => t.RequiresDeposit)
                .HasColumnName("terms_requires_deposit");
                
            terms.Property(t => t.RequiresInsurance)
                .HasColumnName("terms_requires_insurance");
                
            terms.Property(t => t.RestrictedUses)
                .HasColumnName("terms_restricted_uses")
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
                )
                .HasColumnType("jsonb");
        });
    }
}