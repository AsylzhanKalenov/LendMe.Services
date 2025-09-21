using System.Text.Json;
using Lendme.Core.Entities.Booking;
using Lendme.Core.Entities.Catalog;
using Lendme.Core.Entities.ProfileSQLEntities;
using Lendme.Infrastructure.SqlPersistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Lendme.Infrastructure.SqlPersistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Booking Domain
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingFinancials> BookingFinancials { get; set; }
        public DbSet<BookingPayment> BookingPayments { get; set; }
        public DbSet<BookingStatusHistory> BookingStatusHistories { get; set; }
        public DbSet<ItemHandover> ItemHandovers { get; set; }
        // TODO: Consider later
        //public DbSet<DetectedIssue> DetectedIssues { get; set; }

        // Catalog Domain (Assumed entities based on typical catalog structure)
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemDetails> ItemsDetails { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }
        public DbSet<Location> ItemAvailabilities { get; set; }
        public DbSet<RentalTerms> RentalTerms { get; set; }
        
        // Profile Domain
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }
        public DbSet<UserStatistics> UserStatistics { get; set; }
        public DbSet<VerificationDocument> VerificationDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure other entities...
            ConfigureBookingEntities(modelBuilder);
            ConfigureCatalogEntities(modelBuilder);
            
            // Configure Profile entities
            ConfigureProfileEntities(modelBuilder);
        }

        private void ConfigureProfileEntities(ModelBuilder modelBuilder)
        {
            // UserPreferences Configuration
            modelBuilder.Entity<UserPreferences>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                // Foreign key to User
                entity.Property(e => e.ProfileId)
                    .IsRequired();

                // Общие настройки - храним как JSONB
                entity.Property(e => e.General)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<GeneralPreferences>(v, (JsonSerializerOptions)null)
                    )
                    .HasColumnType("jsonb")
                    .HasColumnName("general_preferences");

                // Настройки уведомлений - храним как JSONB
                entity.Property(e => e.Notifications)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<NotificationPreferences>(v, (JsonSerializerOptions)null)
                    )
                    .HasColumnType("jsonb")
                    .HasColumnName("notification_preferences");

                // Настройки приватности - храним как JSONB
                entity.Property(e => e.Privacy)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<PrivacyPreferences>(v, (JsonSerializerOptions)null)
                    )
                    .HasColumnType("jsonb")
                    .HasColumnName("privacy_preferences");

                // Настройки владельца - храним как JSONB
                entity.Property(e => e.OwnerSettings)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<OwnerPreferences>(v, (JsonSerializerOptions)null)
                    )
                    .HasColumnType("jsonb")
                    .HasColumnName("owner_preferences");

                // Настройки арендатора - храним как JSONB
                entity.Property(e => e.RenterSettings)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<RenterPreferences>(v, (JsonSerializerOptions)null)
                    )
                    .HasColumnType("jsonb")
                    .HasColumnName("renter_preferences");

                // Audit fields
                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Indexes
                entity.HasIndex(e => e.ProfileId)
                    .IsUnique() // Один пользователь - одни настройки
                    .HasDatabaseName("IX_UserPreferences_UserId");

                // Table name
                entity.ToTable("user_preferences");
            });

            // Другие конфигурации профиля...
        }

        // Остальные методы конфигурации...
        private void ConfigureBookingEntities(ModelBuilder modelBuilder)
        {
            // Ваша существующая конфигурация букинга
        }

        private void ConfigureCatalogEntities(ModelBuilder modelBuilder)
        {
            // Ваша существующая конфигурация каталога
            modelBuilder.ApplyConfiguration(new ItemDetailsConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Automatically set UpdatedAt for entities that have this property
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity.GetType().GetProperty("UpdatedAt") != null &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}