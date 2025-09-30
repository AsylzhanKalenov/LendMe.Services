using LendMe.Catalog.Infrastructure.SqlPersistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LendMe.Catalog.Infrastructure.SqlPersistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Use your connection string here
        optionsBuilder.UseNpgsql("Host=localhost;Database=lendme_catalog_db;Username=postgres;Password=Aseka220997!;Port=5432",
            npgsqlOptions =>
            {
                npgsqlOptions.UseNetTopologySuite();
                //npgsqlOptions.MigrationsAssembly("LendMe.Catalog.Infrastructure");
            }).UseSnakeCaseNamingConvention();
            
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}