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
            // Seed Booking
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            throw;
        }
    }
}