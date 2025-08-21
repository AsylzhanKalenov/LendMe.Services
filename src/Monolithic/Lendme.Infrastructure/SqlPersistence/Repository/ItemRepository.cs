using Lendme.Core.Entities.Catalog;
using Lendme.Core.Interfaces;
using Lendme.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lendme.Infrastructure.SqlPersistence.Repository;

public class ItemRepository : IItemRepository
{
    private readonly ApplicationDbContext _context;

    public ItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Item> AddAsync(Item item, CancellationToken cancellationToken)
    {
        await _context.Items.AddAsync(item, cancellationToken);
        return item;
    }

    public Task<IQueryable<Item>> GetQueryableAsync()
    {
        return Task.FromResult(_context.Items.AsQueryable());
    }

    public async Task<Item> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Items
            .Include(i => i.Details)
            .FirstOrDefaultAsync(i => i.Id == id && !i.IsDeleted, cancellationToken);
    }

    public async Task<List<Item>> GetAllWithDetailsAsync(int skip, int take, CancellationToken cancellationToken)
    {
        return await _context.Items
            .Include(i => i.Details)
            .Where(i => !i.IsDeleted)
            .OrderByDescending(i => i.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken)
    {
        return await _context.Items
            .Where(i => !i.IsDeleted)
            .CountAsync(cancellationToken);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }
}