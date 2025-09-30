using LendMe.Catalog.Core.Entity;
using LendMe.Catalog.Infrastructure.SqlPersistence.Context;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace LendMe.Catalog.Web.Feature.Seeder;

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
            Guid[] itemIds = new Guid[8];
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
                    ) { Id = itemIds[0] = Guid.Parse("10000000-0000-0000-0000-000000000001") },
                    new Item(
                        "MacBook Pro 16\"",
                        100m,
                        600m,
                        2000m,
                        5000m,
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                    ) { Id = itemIds[1] = Guid.Parse("10000000-0000-0000-0000-000000000002") },
                    new Item(
                        "Горный велосипед Trek",
                        25m,
                        150m,
                        500m,
                        1200m,
                        Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
                    ) { Id = itemIds[2] = Guid.Parse("10000000-0000-0000-0000-000000000003") },
                    new Item(
                        "Дрель Bosch Professional",
                        20m,
                        120m,
                        400m,
                        800m,
                        Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")
                    ) { Id = itemIds[3] = Guid.Parse("10000000-0000-0000-0000-000000000004") },
                    new Item(
                        "Электросамокат Xiaomi",
                        20m,
                        120m,
                        400m,
                        1000m,
                        Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                    ) { Id = itemIds[4] = Guid.Parse("10000000-0000-0000-0000-000000000005") },
                    new Item(
                        "Sony PlayStation 5",
                        30m,
                        180m,
                        600m,
                        1500m,
                        Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")
                    ) { Id = itemIds[5] = Guid.Parse("10000000-0000-0000-0000-000000000006") },
                    new Item(
                        "Сноуборд Burton",
                        40m,
                        240m,
                        800m,
                        2000m,
                        Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")
                    ) { Id = itemIds[6] = Guid.Parse("10000000-0000-0000-0000-000000000007") },
                    new Item(
                        "Смокинг Hugo Boss",
                        45m,
                        270m,
                        900m,
                        2500m,
                        Guid.Parse("55555555-5555-5555-5555-555555555555"),
                        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")
                    ) { Id = itemIds[7] = Guid.Parse("10000000-0000-0000-0000-000000000008") }
                };

                // Устанавливаем статус Active для всех items
                foreach (var item in items)
                {
                    item.UpdateStatus(ItemStatus.Active);
                }

                await _context.Items.AddRangeAsync(items);
                await _context.SaveChangesAsync();
                Console.WriteLine("Items seeded successfully.");
            }
            else
            {
                // Получаем существующие ID items для связывания с Rent
                var existingItems = await _context.Items.Select(i => i.Id).ToArrayAsync();
                for (int i = 0; i < Math.Min(itemIds.Length, existingItems.Length); i++)
                {
                    itemIds[i] = existingItems[i];
                }
            }

           // Seed Rents
            Guid[] rentIds = new Guid[5];
            if (!await _context.Rents.AnyAsync())
            {
                var geometryFactory = new GeometryFactory();
                
                var rents = new[]
                {
                    new Rent
                    {
                        Id = rentIds[0] = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                        Type = "Point",
                        MinPrice = 20,
                        MaxPrice = 100,
                        Longitude = 76.9286,
                        Latitude = 43.2220,
                        Address = "проспект Республики, 15",
                        City = "Алматы",
                        District = "Алмалинский",
                        RadiusMeters = 5000,
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-30),
                        Points = geometryFactory.CreatePoint(new Coordinate(76.9286, 43.2220)) // Центр города (Республика)
                    },
                    new Rent
                    {
                        Id = rentIds[1] = Guid.Parse("20000000-0000-0000-0000-000000000002"),
                        Type = "Point",
                        MinPrice = 30,
                        MaxPrice = 200,
                        Longitude = 76.9755,
                        Latitude = 43.2630,
                        Address = "проспект Достык, 128",
                        City = "Алматы",
                        District = "Медеуский",
                        RadiusMeters = 3000,
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-25),
                        Points = geometryFactory.CreatePoint(new Coordinate(76.9755, 43.2630)) // Достык (~6 км к юго-востоку)
                    },
                    new Rent
                    {
                        Id = rentIds[2] = Guid.Parse("20000000-0000-0000-0000-000000000003"),
                        Type = "Point",
                        MinPrice = 15,
                        MaxPrice = 80,
                        Longitude = 76.8547,
                        Latitude = 43.2565,
                        Address = "улица Толе би, 59",
                        City = "Алматы",
                        District = "Ауэзовский",
                        RadiusMeters = 7000,
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-20),
                        Points = geometryFactory.CreatePoint(new Coordinate(76.8547, 43.2565)) // Толе би (~7 км к западу)
                    },
                    new Rent
                    {
                        Id = rentIds[3] = Guid.Parse("20000000-0000-0000-0000-000000000004"),
                        Type = "Area",
                        MinPrice = 25,
                        MaxPrice = 150,
                        Longitude = 76.9200,
                        Latitude = 43.1850,
                        Address = "улица Шевченко, 118",
                        City = "Алматы",
                        District = "Бостандыкский",
                        RadiusMeters = 10000,
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-15),
                        Points = geometryFactory.CreatePoint(new Coordinate(76.9200, 43.1850)) // Шевченко (~4 км к югу)
                    },
                    new Rent
                    {
                        Id = rentIds[4] = Guid.Parse("20000000-0000-0000-0000-000000000005"),
                        Type = "Point",
                        MinPrice = 40,
                        MaxPrice = 300,
                        Longitude = 76.8900,
                        Latitude = 43.2800,
                        Address = "проспект Райымбека, 348",
                        City = "Алматы",
                        District = "Жетысуский",
                        RadiusMeters = 2000,
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-10),
                        Points = geometryFactory.CreatePoint(new Coordinate(76.8900, 43.2800)) // Райымбека (~8 км к северо-западу)
                    }
                };

                await _context.Rents.AddRangeAsync(rents);
                await _context.SaveChangesAsync();
                Console.WriteLine("Rents seeded successfully.");
            }
            else
            {
                // Получаем существующие ID rents
                var existingRents = await _context.Rents.Select(r => r.Id).ToArrayAsync();
                for (int i = 0; i < Math.Min(rentIds.Length, existingRents.Length); i++)
                {
                    rentIds[i] = existingRents[i];
                }
            }
            
            // Seed RentItems (связи между Rent и Item)
            if (!await _context.RentItems.AnyAsync())
            {
                var rentItems = new List<RentItems>();

                // Rent 1 (Москва, центр) - iPhone, MacBook, PlayStation
                rentItems.AddRange(new[]
                {
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[0],
                        ItemId = itemIds[0], // iPhone 15 Pro
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-29)
                    },
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[0],
                        ItemId = itemIds[1], // MacBook Pro
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-29)
                    },
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[0],
                        ItemId = itemIds[5], // PlayStation 5
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-28)
                    }
                });

                // Rent 2 (СПб) - Велосипед, Сноуборд
                rentItems.AddRange(new[]
                {
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[1],
                        ItemId = itemIds[2], // Велосипед Trek
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-24)
                    },
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[1],
                        ItemId = itemIds[6], // Сноуборд Burton
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-23)
                    }
                });

                // Rent 3 (Краснодар) - Дрель, Самокат
                rentItems.AddRange(new[]
                {
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[2],
                        ItemId = itemIds[3], // Дрель Bosch
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-19)
                    },
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[2],
                        ItemId = itemIds[4], // Самокат Xiaomi
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-18)
                    }
                });

                // Rent 4 (Екатеринбург) - Смокинг, iPhone
                rentItems.AddRange(new[]
                {
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[3],
                        ItemId = itemIds[7], // Смокинг Hugo Boss
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-14)
                    },
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[3],
                        ItemId = itemIds[0], // iPhone 15 Pro (может быть в нескольких локациях)
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-13)
                    }
                });

                // Rent 5 (Москва, Арбат) - MacBook, Сноуборд
                rentItems.AddRange(new[]
                {
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[4],
                        ItemId = itemIds[1], // MacBook Pro
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-9)
                    },
                    new RentItems
                    {
                        Id = Guid.NewGuid(),
                        RentId = rentIds[4],
                        ItemId = itemIds[6], // Сноуборд Burton
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-8)
                    }
                });

                await _context.RentItems.AddRangeAsync(rentItems);
                await _context.SaveChangesAsync();
                Console.WriteLine("RentItems seeded successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            throw;
        }
    }

    public async Task ClearDatabaseAsync()
    {
        try
        {
            // Удаляем в правильном порядке из-за внешних ключей
            var rentItems = await _context.RentItems.ToListAsync();
            _context.RentItems.RemoveRange(rentItems);

            var rents = await _context.Rents.ToListAsync();
            _context.Rents.RemoveRange(rents);

            var items = await _context.Items.ToListAsync();
            _context.Items.RemoveRange(items);
            
            var categories = await _context.Categories.ToListAsync();
            _context.Categories.RemoveRange(categories);
            
            await _context.SaveChangesAsync();
            Console.WriteLine("Database cleared successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing database: {ex.Message}");
            throw;
        }
    }

    public async Task ClearAndSeedAsync()
    {
        await ClearDatabaseAsync();
        await SeedAsync();
        Console.WriteLine("Database cleared and seeded successfully.");
    }
}