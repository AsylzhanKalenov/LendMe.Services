using LendMe.Catalog.Core.Entity;

namespace LendMe.Catalog.Core.Interfaces.Repository;

public interface IItemRepository
{
    Task<Item> AddAsync(Item item, CancellationToken cancellationToken);
    Item Update(Item item);
    Task<IQueryable<Item>> GetQueryableAsync();
    Task<Item> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Item>> GetAllWithDetailsAsync(int skip, int take, CancellationToken cancellationToken);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}