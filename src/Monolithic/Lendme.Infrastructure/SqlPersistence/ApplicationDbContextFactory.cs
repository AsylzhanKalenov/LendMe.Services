using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lendme.Infrastructure.SqlPersistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use your connection string here
        optionsBuilder.UseNpgsql("Host=localhost;Database=lendme_db;Username=postgres;Password=Aseka220997!;Port=5432");
            
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}