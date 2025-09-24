using System.Text.Json;
using Lendme.Core.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendme.Infrastructure.SqlPersistence.Configurations;

public class ItemDetailsConfiguration : IEntityTypeConfiguration<ItemDetails>
{
    public void Configure(EntityTypeBuilder<ItemDetails> builder)
    {
        builder.ToTable("ItemDetails");
        
        builder.HasKey(x => x.Id);
        
        // Базовые свойства
        builder.Property(x => x.Id)
            .ValueGeneratedNever(); // Генерируется в конструкторе
            
        builder.Property(x => x.ItemId)
            .IsRequired();
            
        builder.Property(x => x.Description)
            .HasMaxLength(2000);
        
        // Список тегов как JSON
        builder.Property(x => x.Tags)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
            )
            .HasColumnType("jsonb");

        

        // Индексы для производительности
        builder.HasIndex(x => x.ItemId)
            .HasDatabaseName("IX_ItemDetails_ItemId");
            
        // builder.HasIndex(x => new { x.Location.Latitude, x.Location.Longitude })
        //     .HasDatabaseName("IX_ItemDetails_Location")
        //     .HasDatabaseName("IX_ItemDetails_Location_Coordinates");

        // Связь с основной сущностью Item (если есть)
        builder.HasOne<Item>()
            .WithOne()
            .HasForeignKey<ItemDetails>(x => x.ItemId);
    }
}