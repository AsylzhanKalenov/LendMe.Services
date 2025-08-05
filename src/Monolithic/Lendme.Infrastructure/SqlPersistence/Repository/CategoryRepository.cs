using Lendme.Core.Entities.Catalog;
using Lendme.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lendme.Infrastructure.SqlPersistence.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<Category>> GetCategoriesAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Categories.ToListAsync(cancellationToken);
    }
}