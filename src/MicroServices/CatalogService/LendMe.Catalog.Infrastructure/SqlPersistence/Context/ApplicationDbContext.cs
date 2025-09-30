using LendMe.Catalog.Core.Entity;
using Microsoft.EntityFrameworkCore;
using EFCore.NamingConventions;

namespace LendMe.Catalog.Infrastructure.SqlPersistence.Context;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<RentItems> RentItems { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureCatalogEntities(modelBuilder);
        
        modelBuilder.HasPostgresExtension("pg_trgm");

        // Конфигурация Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.Name);
        });

        // Конфигурация Item
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DailyPrice).HasPrecision(10, 2);
            entity.Property(e => e.WeeklyPrice).HasPrecision(10, 2);
            entity.Property(e => e.MonthlyPrice).HasPrecision(10, 2);
            entity.Property(e => e.DepositAmount).HasPrecision(10, 2);
            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.Status).HasDefaultValue(ItemStatus.Active);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Индексы
            entity.HasIndex(e => e.CategoryId).HasFilter("is_deleted = false");
            entity.HasIndex(e => e.DailyPrice).HasFilter("is_deleted = false");
            entity.HasIndex(e => e.IsAvailable).HasFilter("is_deleted = false");
            entity.HasIndex(e => e.Status).HasFilter("is_deleted = false");
            entity.HasIndex(e => e.CreatedAt).IsDescending().HasFilter("is_deleted = false");

            
            // Полнотекстовый поиск
            entity.HasIndex(e => e.Title)
                .HasMethod("gin")
                .HasOperators("gin_trgm_ops");
        });

        // Конфигурация Rent
        modelBuilder.Entity<Rent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Type).HasDefaultValue("Point");
            entity.Property(e => e.Longitude).IsRequired();
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.District).HasMaxLength(100);
            entity.Property(e => e.RadiusMeters).HasDefaultValue(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Конфигурация для PostGIS Point
            entity.Property(e => e.Points)
                .HasColumnType("geography(POINT, 4326)")
                .HasColumnName("location");

            // Пространственный индекс
            entity.HasIndex(e => e.Points)
                .HasMethod("gist");
            
            entity.HasIndex(e => e.City);
            entity.HasIndex(e => e.District);
        });

        // Конфигурация RentItems
        modelBuilder.Entity<RentItems>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.Item)
                .WithMany()
                .HasForeignKey(e => e.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Rent)
                .WithMany()
                .HasForeignKey(e => e.RentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.ItemId, e.RentId }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
    
    private void ConfigureCatalogEntities(ModelBuilder modelBuilder)
    {
        // Ваша существующая конфигурация каталога
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}