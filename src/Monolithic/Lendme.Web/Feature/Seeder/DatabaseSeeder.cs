using Lendme.Core.Entities.Catalog;
using Lendme.Infrastructure.SqlPersistence;
using Microsoft.EntityFrameworkCore;

namespace Lendme.Web.Feature.Seeder;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;

    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Seed Categories
            if (!await _context.Categories.AnyAsync())
            {
                var categories = new[]
                {
                    new Category 
                    { 
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), 
                        Name = "Электроника", 
                        Description = "Электронные устройства и гаджеты" 
                    },
                    new Category 
                    { 
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), 
                        Name = "Спорт", 
                        Description = "Спортивное оборудование и инвентарь" 
                    },
                    new Category 
                    { 
                        Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), 
                        Name = "Инструменты", 
                        Description = "Строительные и ремонтные инструменты" 
                    },
                    new Category 
                    { 
                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), 
                        Name = "Транспорт", 
                        Description = "Автомобили, велосипеды, самокаты" 
                    },
                    new Category 
                    { 
                        Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), 
                        Name = "Одежда", 
                        Description = "Одежда и аксессуары для особых случаев" 
                    }
                };

                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
                Console.WriteLine("Categories seeded successfully.");
            }

            // Seed Items
            if (!await _context.Items.AnyAsync())
            {
                var items = new[]
                {
                    new Item(
                        "iPhone 15 Pro",
                        50m,
                        300m,
                        1000m,
                        2000m,
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                    ),
                    new Item(
                        "MacBook Pro 16\"",
                        100m,
                        600m,
                        2000m,
                        5000m,
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                    ),
                    new Item(
                        "Горный велосипед Trek",
                        25m,
                        150m,
                        500m,
                        1200m,
                        Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
                    ),
                    new Item(
                        "Дрель Bosch Professional",
                        20m,
                        120m,
                        400m,
                        800m,
                        Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")
                    ),
                    new Item(
                        "Электросамокат Xiaomi",
                        20m,
                        120m,
                        400m,
                        1000m,
                        Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                    )
                };

                // Устанавливаем статус Active для некоторых items
                items[0].UpdateStatus(ItemStatus.Active);
                items[1].UpdateStatus(ItemStatus.Active);
                items[2].UpdateStatus(ItemStatus.Active);
                items[3].UpdateStatus(ItemStatus.Active);
                items[4].UpdateStatus(ItemStatus.Active);

                await _context.Items.AddRangeAsync(items);
                await _context.SaveChangesAsync();
                Console.WriteLine("Items seeded successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            throw;
        }
    }
}