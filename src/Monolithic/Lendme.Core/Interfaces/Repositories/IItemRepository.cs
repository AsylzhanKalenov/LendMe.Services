using Lendme.Core.Entities.Catalog;

namespace Lendme.Core.Interfaces.Repositories;

public interface IItemRepository
{
    Task<Item> AddAsync(Item item, CancellationToken cancellationToken);
    Task<IQueryable<Item>> GetQueryableAsync();
    Task<Item> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Item>> GetAllWithDetailsAsync(int skip, int take, CancellationToken cancellationToken);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}