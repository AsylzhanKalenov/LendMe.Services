using Lendme.Core.Entities.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lendme.Infrastructure.SqlPersistence.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");
        
        // BookingFinancials как вложенный объект
        builder.OwnsOne(b => b.Financials, financials =>
        {
            financials.Property(f => f.ItemPrice).HasColumnName("item_price");
            financials.Property(f => f.RentalDays).HasColumnName("rental_days");
            financials.Property(f => f.DailyRate).HasColumnName("DailyRate");
            financials.Property(f => f.SubTotal).HasColumnName("SubTotal");
            financials.Property(f => f.DiscountAmount).HasColumnName("DiscountAmount");
            financials.Property(f => f.DiscountPercentage).HasColumnName("DiscountPercentage");
            financials.Property(f => f.TotalAmount).HasColumnName("TotalAmount");
            financials.Property(f => f.DepositAmount).HasColumnName("DepositAmount");
            financials.Property(f => f.Currency).HasColumnName("Currency");
            financials.Property(f => f.PenaltyAmount).HasColumnName("PenaltyAmount");
            financials.Property(f => f.RefundAmount).HasColumnName("RefundAmount");
            financials.Property(f => f.DamageCompensation).HasColumnName("DamageCompensation");
        });
        
        // Игнорируем навигационные свойства
        builder.Ignore(b => b.StatusHistory);
        builder.Ignore(b => b.Payments);
    }
}